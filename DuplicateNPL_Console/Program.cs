﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuplicateNPL_BusinessLayer;
namespace DuplicateNPL_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            DeDuplication deDuplication = new DeDuplication();
            deDuplication.CheckDuplicateNPLReferences(args[0].ToString(), args[1].ToString());
        }
    }
}
