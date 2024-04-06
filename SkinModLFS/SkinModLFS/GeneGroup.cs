using System.Collections.Generic;
using System.Xml;
using Verse;

namespace LFSGenesExpandedSkin
{
    public class GeneChances
    {
        public GeneDef geneDef;

        public GeneChances(GeneDef geneDef, float chance)
        {
            this.geneDef = geneDef;
        }

        public GeneChances()
        {
        }

        public void LoadDataFromXmlCustom(XmlNode xmlRoot)
        {
            DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "geneDef", xmlRoot.Name, null, null, null);
        }
    }

    public class GeneGroup
    {
        public GeneGroup()
        {
        }

        public List<GeneChances> Endogenes = new List<GeneChances>();
        public GeneGroup(List<GeneChances> endogeneChances)
        {
            this.Endogenes = endogeneChances;
        }
    }

    public class GeneGroups : Def
    {
        public List<GeneGroup> geneGroups = new List<GeneGroup>();
    }
}