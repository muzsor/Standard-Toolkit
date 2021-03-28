﻿namespace Krypton.Toolkit
{
    /// <summary>
    /// Custom type converter so that PaletteMode values appear as neat text at design time.
    /// </summary>
    internal class PaletteModeConverter : StringLookupConverter
    {
        #region Static Fields
        private Pair[] _pairs = new Pair[] { new Pair(PaletteMode.ProfessionalSystem,    "Professional - System"),
                                             new Pair(PaletteMode.ProfessionalOffice2003,"Professional - Office 2003"),
                                             new Pair(PaletteMode.Office2007Blue,        "Office 2007 - Blue"),
                                             new Pair(PaletteMode.Office2007Silver,      "Office 2007 - Silver"),
                                             new Pair(PaletteMode.Office2007White,       "Office 2007 - White"),
                                             new Pair(PaletteMode.Office2007Black,       "Office 2007 - Black"),
                                             new Pair(PaletteMode.Office2010Blue,        "Office 2010 - Blue"),
                                             new Pair(PaletteMode.Office2010Silver,      "Office 2010 - Silver"),
                                             new Pair(PaletteMode.Office2010White,       "Office 2010 - White"),
                                             new Pair(PaletteMode.Office2010Black,       "Office 2010 - Black"),
                                             new Pair(PaletteMode.Office2013White,       "Office 2013 - White"),
                                             new Pair(PaletteMode.Office365Black,        "Office 365 - Black"),
                                             new Pair(PaletteMode.Office365Blue,         "Office 365 - Blue"),
                                             new Pair(PaletteMode.Office365Silver,       "Office 365 - Silver"),
                                             new Pair(PaletteMode.Office365White,        "Office 365 - White"),
                                             new Pair(PaletteMode.SparkleBlue,           "Sparkle - Blue"),
                                             new Pair(PaletteMode.SparkleOrange,         "Sparkle - Orange"),
                                             new Pair(PaletteMode.SparklePurple,         "Sparkle - Purple") };
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the PaletteModeConverter class.
        /// </summary>
        public PaletteModeConverter()
            : base(typeof(PaletteMode))
        {
        }
        #endregion

        #region Protected
        /// <summary>
        /// Gets an array of lookup pairs.
        /// </summary>
        protected override Pair[] Pairs => _pairs;

        #endregion
    }
}
