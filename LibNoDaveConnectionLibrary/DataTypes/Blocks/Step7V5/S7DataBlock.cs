﻿/*
 This implements a high level Wrapper between libnodave.dll and applications written
 in MS .Net languages.
 
 This ConnectionLibrary was written by Jochen Kuehner
 * http://jfk-solutuions.de/
 * 
 * Thanks go to:
 * Steffen Krayer -> For his work on MC7 decoding and the Source for his Decoder
 * Zottel         -> For LibNoDave

 WPFToolboxForSiemensPLCs is free software; you can redistribute it and/or modify
 it under the terms of the GNU Library General Public License as published by
 the Free Software Foundation; either version 2, or (at your option)
 any later version.

 WPFToolboxForSiemensPLCs is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU Library General Public License
 along with Libnodave; see the file COPYING.  If not, write to
 the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.  
*/
using System;
using System.Collections.Generic;
using System.Linq;

using DotNetSiemensPLCToolBoxLibrary.PLCs.S7_xxx.MC7;

namespace DotNetSiemensPLCToolBoxLibrary.DataTypes.Blocks.Step7V5
{
    [Serializable()]
    public class S7DataBlock : S7Block, IDataBlock
    {
        public override IEnumerable<String> Dependencies
        {
            get
            {
                var retVal = new List<String>();

                retVal.AddRange(((S7DataRow)Structure).Dependencies);

                return retVal.Distinct().OrderBy(itm => itm);
            }
        }

        public int FBNumber { get; set;}  //If it is a Instance DB
        public bool IsInstanceDB { get; set; }

        public IDataRow Structure
        {
            get
            {
                if (StructureFromString != null) 
                    return StructureFromString;
                return StructureFromMC7;
            }
            set
            {
                StructureFromString = (S7DataRow)Structure;
                StructureFromMC7 = (S7DataRow)Structure;
            }
        }

        public S7DataRow StructureFromString { get; set; }
        public S7DataRow StructureFromMC7 { get; set; }

        /// <summary>
        /// With this function you get the Structure with expanden Arrays!
        /// </summary>
        /// <returns></returns>
        public IDataRow GetArrayExpandedStructure(S7DataBlockExpandOptions myExpOpt)
        {
            return ((S7DataRow)Structure)._GetExpandedChlidren(myExpOpt)[0];
        }

        public S7DataRow GetDataRowWithAddress(ByteBitAddress address)
        {
            var allRw = this.GetArrayExpandedStructure();
            return S7DataRow.GetDataRowWithAddress((S7DataRow)allRw, address);
        }

        private S7DataRow expStruct = null;

        public IDataRow GetArrayExpandedStructure()
        {
            //Todo: Vergleich der Expand Options, und beim änderen eines inneren wertes des blocks, diesen löschen (erst bei schreibsup wichtig!)
            if (expStruct != null) 
                return expStruct;
            return expStruct = (S7DataRow)GetArrayExpandedStructure(new S7DataBlockExpandOptions());
        }

        public override string ToString()
        {
            string retVal = "";
            if (this.BlockType == PLCBlockType.UDT)
                retVal += "UDT";
            else
                retVal += "DB";
            retVal += BlockNumber.ToString() + Environment.NewLine;
            if (Structure != null)
                retVal += Structure.ToString();
            return retVal;
        }       
    }
}
