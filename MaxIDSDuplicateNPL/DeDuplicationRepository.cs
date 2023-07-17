using DuplicateNPL_Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateNPL_Repository
{
    public class DeDuplicationRepository
    {
        /// <summary>
        /// Get the citation details for all NPL records
        /// </summary>
        /// <returns></returns>
        public List<NplModel> GetNPLRecordCitation()
        {
            List<NplModel> nplModels = new List<NplModel>();
            using (maxids_duplicateNPL_v4Entities entities = new maxids_duplicateNPL_v4Entities())
            {
                nplModels = ((from oth in entities.tbl_record_others
                              where oth.isactive == 1
                              select new NplModel()
                              {
                                  RecordId = oth.record_id.ToString(),
                                  Citation = oth.citation,
                                  PlainCitation = oth.plaincitation,
                                  IsDuplicateCheck = oth.IsDuplicateCheck ?? 0,
                                  id = oth.record_other_id
                              }).Union(
                    from jour in entities.tbl_record_journals
                    where jour.isactive == 1
                    select new NplModel()
                    {
                        RecordId = jour.record_id.ToString(),
                        Citation = jour.citation,
                        PlainCitation = jour.plaincitation,
                        IsDuplicateCheck = jour.IsDuplicateCheck ?? 0,
                        id = jour.record_journal_id
                    })).OrderByDescending(x => x.RecordId).ToList();
            }
            return nplModels;
        }

        /// <summary>
        /// Get the NPL Duplicate Config values
        /// </summary>
        /// <returns></returns>
        public List<NplSettingModel> GetNPLDuplicateConfig()
        {
            List<NplSettingModel> nplModels = new List<NplSettingModel>();
            using (maxids_duplicateNPL_v4Entities entities = new maxids_duplicateNPL_v4Entities())
            {
                nplModels = (from con in entities.tbl_npl_duplicate_config
                             where con.IsActive == true
                             select new NplSettingModel()
                             {
                                 ExcludedWords = con.ExcludedWords,
                                 Percentage = con.Percentage,
                             }).ToList();
            }
            return nplModels;
        }


        /// <summary>
        /// Save the matching score for NPL Records
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="matchingRecordID"></param>
        /// <param name="score"></param>
        /// <param name="isDuplicate"></param>
        public bool DoSaveDuplicateNPLScores(int recordId, int matchingRecordID, int score, bool isDuplicate = true, int userId = 1)
        {
            bool isSaved = false;
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["MaxIds_DuplicateNPLEntities"]))
            {
                using (SqlCommand sqlCommand = new SqlCommand("USP_DoSaveNPLDuplicateScore", sqlConnection))
                {
                    sqlConnection.Open();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@RecordID", recordId);
                    sqlCommand.Parameters.AddWithValue("@MatchingRecordIds", matchingRecordID);
                    sqlCommand.Parameters.AddWithValue("@Score", recordId == matchingRecordID ? 100 : score);
                    sqlCommand.Parameters.AddWithValue("@isDuplicate", isDuplicate);
                    sqlCommand.Parameters.AddWithValue("@userId", userId);
                    sqlCommand.CommandTimeout = 0;
                    sqlCommand.ExecuteNonQuery();
                    isSaved = true;
                }
            }
            return isSaved;
        }
    }
}
