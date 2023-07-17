using DuplicateNPL_Common;
using DuplicateNPL_Model;
using DuplicateNPL_Repository;
using FuzzySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DuplicateNPL_BusinessLayer
{
    public class DeDuplication
    {
        DeDuplicationRepository duplicationRepository = new DeDuplicationRepository();
        public void CheckDuplicateNPLReferences(string SourceType, string inputId)
        {
            List<NplDuplicateModel> matchingRecordScores = new List<NplDuplicateModel>();
            try
            {

                List<NplModel> nplList = duplicationRepository.GetNPLRecordCitation();
                List<NplSettingModel> nplSettings = duplicationRepository.GetNPLDuplicateConfig();

                string[] dontCheck = nplSettings.Select(x => x.ExcludedWords).ToArray();
                int? matchingPercentage = nplSettings.Select(x => x.Percentage).FirstOrDefault();

                List<NplModel> inputNPLlist = (InputText.Count > 0 && !OnceOffRequired) ? InputText : nplList.Where(x => x.IsDuplicateCheck == 0).ToList();
                string pattern = "[~!@#$%^&*_+<>{}(),\".-]|(" + string.Join("|", dontCheck) + ")";

                if (nplList.Count > 0 && inputNPLlist.Count > 0)
                {
                    ParallelOptions opts = new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.65) * 1.0)) };
                    Parallel.ForEach(inputNPLlist, opts, item =>
                    {
                        Regex regex = new Regex(UtilityConstants.RegexPatterns.OnlyAlphabetDigits);
                        Regex doiRegex = new Regex(UtilityConstants.RegexPatterns.DOIRegex);
                        Regex alphaNumericRegex = new Regex(UtilityConstants.RegexPatterns.AlphaNumericRegex);
                        List<NplModel> supplierListQuery = new List<NplModel>();

                        string strDOI = PopulateDOIString(item.Citation, ' ', doiRegex);
                        string applicationNumber = PopulateApplicationNumberBasedOnKeyword(item.Citation, ' ', regex, alphaNumericRegex);
                        string[] str1Words = Regex.Replace(item.Citation, pattern, "", RegexOptions.IgnoreCase).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        List<string> str1Wordss = str1Words.Where(x => x.Length > 4).ToList();

                        if (!string.IsNullOrWhiteSpace(applicationNumber))
                        {
                            supplierListQuery = nplList.Where(x => x.Citation.IndexOf(applicationNumber, StringComparison.OrdinalIgnoreCase) >= 0).Select(x => x).ToList();
                        }
                        else if (!string.IsNullOrWhiteSpace(strDOI))
                        {
                            supplierListQuery = nplList.Where(x => x.Citation.IndexOf(strDOI, StringComparison.OrdinalIgnoreCase) >= 0).Select(x => x).ToList();
                        }
                        else
                        {
                            supplierListQuery = nplList.Where(x => str1Wordss.Any(y => Regex.Replace(x.Citation, pattern, "", RegexOptions.IgnoreCase).Contains(y) && !dontCheck.Contains(y))).ToList();
                        }

                        var macthingScores = supplierListQuery.Select(x => new { item.RecordId, x.Citation, x.PlainCitation, MatchingRecord = x.RecordId, Score = Fuzz.WeightedRatio(Regex.Replace(item.Citation.Replace("&amp;", "").RemoveSpecialCharacters(), pattern, "", RegexOptions.IgnoreCase), Regex.Replace(x.Citation.Replace("&amp;", "").RemoveSpecialCharacters(), pattern, "", RegexOptions.IgnoreCase), FuzzySharp.PreProcess.PreprocessMode.Full) }).ToList();

                        macthingScores.ForEach(record =>
                        //  foreach (var record in macthingScores)
                        {
                            if (record.Score >= matchingPercentage)
                            {
                                matchingRecordScores.Add(new NplDuplicateModel() { RecordId = item.RecordId, Citation = record.Citation, MatchingRecord = record.MatchingRecord, Score = record.Score, IsDuplicate = true, plaincitation = record.PlainCitation });
                                if (item.RecordId.Length > 1)
                                {
                                    bool isSaved = duplicationRepository.DoSaveDuplicateNPLScores(int.Parse(item.RecordId), int.Parse(record.MatchingRecord), record.Score, true);
                                }
                            }
                        });
                    });
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// To extract the application number from citation text
        /// </summary>
        /// <param name="citation"></param>
        /// <param name="whitesSpace"></param>
        /// <param name="regex"></param>
        /// <param name="alphaNumericRegex"></param>
        /// <returns></returns>
        private static string PopulateApplicationNumberBasedOnKeyword(string citation, char whitesSpace, Regex regex, Regex alphaNumericRegex)
        {
            string strApplicationNumber = string.Empty;
            if (UtilityConstants.Collections.ApplicationNoSearches.Any(searchData => citation.ToLower().Contains(searchData)))
            {
                UtilityConstants.Collections.ApplicationNoSearches.ToList().ForEach(searchData =>
                {
                    if (string.IsNullOrWhiteSpace(strApplicationNumber)
                        && !string.IsNullOrWhiteSpace(searchData)
                        && citation.ToLower().Contains(searchData))
                    {
                        string[] splittedCitation = citation.ToLower().Split(new string[] { searchData }, StringSplitOptions.RemoveEmptyEntries);
                        if (splittedCitation != null && splittedCitation.Length > 1)
                        {
                            foreach (string word in splittedCitation[1].Split(whitesSpace).ToList())
                            {
                                try
                                {
                                    if (!string.IsNullOrWhiteSpace(word))
                                    {
                                        string tempData = regex.Replace(word, string.Empty);
                                        // Scenario1: Consider all are digits then it should be application number
                                        if (tempData.Replace("/", "").Replace(",", "").Replace(".", "").All(char.IsLetterOrDigit))
                                        {
                                            strApplicationNumber = tempData;
                                            break;
                                        }
                                        // Scenario2: Consider application number that starts with PCT, WO, US
                                        if (tempData.StartsWith("pct")
                                            || tempData.StartsWith("wo")
                                            || tempData.StartsWith("us")
                                            || alphaNumericRegex.IsMatch(tempData))
                                        {
                                            strApplicationNumber = tempData.ToUpper();
                                            break;
                                        }
                                    }
                                }
                                catch
                                {
                                    strApplicationNumber = null;
                                }
                            }
                        }
                    }
                });
            }
            return strApplicationNumber;
        }

        /// <summary>
        /// To Extract the DOI from Citation text
        /// </summary>
        /// <param name="citation"></param>
        /// <param name="whitesSpace"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        private static string PopulateDOIString(string citation, char whitesSpace, Regex regex)
        {
            string strDOINumber = string.Empty;
            if (UtilityConstants.Collections.ApplicationNoSearches.Any(searchData => citation.ToLower().Contains(searchData)))
            {
                UtilityConstants.Collections.ApplicationNoSearches.ToList().ForEach(searchData =>
                {
                    if (string.IsNullOrWhiteSpace(strDOINumber)
                        && !string.IsNullOrWhiteSpace(searchData)
                        && citation.ToLower().Contains(searchData))
                    {
                        string[] splittedCitation = citation.ToLower().Split(new string[] { searchData }, StringSplitOptions.RemoveEmptyEntries);
                        if (splittedCitation != null && splittedCitation.Length > 1)
                        {
                            foreach (string word in splittedCitation[1].Split(whitesSpace).ToList())
                            {
                                try
                                {
                                    if (!string.IsNullOrWhiteSpace(word))
                                    {
                                        string tempData = regex.Replace(word, string.Empty);
                                        // Scenario1: Consider all are digits then it should be application number
                                        if (!string.IsNullOrWhiteSpace(tempData))
                                        {
                                            strDOINumber = tempData;
                                            break;
                                        }
                                    }
                                }
                                catch
                                {
                                    strDOINumber = null;
                                }
                            }
                        }
                    }
                });
            }
            return strDOINumber;
        }
    }
}
