
using DuplicateNPL_BusinessLayer;

namespace DuplicateNPL_Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /***
             * Initiates document matching reference request for import with references sync
             * args[0] - Page Name (TOC, Import With References, Bulk Upload)
             * args[1] - Input Id
             * ***/
            DeDuplication duplication = new DeDuplication();
            duplication.CheckDuplicateNPLReferences(args[0].ToString(), args[1].ToString());
        }
    }
}
