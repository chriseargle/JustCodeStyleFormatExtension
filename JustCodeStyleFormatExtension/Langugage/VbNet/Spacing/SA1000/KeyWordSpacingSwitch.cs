﻿namespace JustCodeStyleFormatExtension.Langugage.VbNet.Spacing.SA1000
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using StyleFormatEngine.Extensions;
    using StyleFormatEngine.Helpers.Spacing.AddSpacing;
    using Telerik.JustCode.CommonLanguageModel;

    /// <summary> 
    /// 
    /// Following style cop enforced rule SA1000: Spacing around keywords Switch
    /// 
    /// </summary>
    [Export(typeof(IEngineModule))]
    [Export(typeof(ICodeMarkerGroupDefinition))]
    public class KeyWordSpacingSwitch : CodeMarkerProviderModuleBase
    {
        private readonly AddSpaceHelper whiteSpaceHelper = new AddSpaceHelper();

        private const string WarningId = "SA1000A-VB-Switch";
        private const string MarkerText = "VB - Spacing around keyword \"Switch\" should be spaced correctly";
        private const string Description = "VB - Spacing around keyword \"Switch\" should be spaced correctly";
        private const string FixText = "VB - Spacing around keyword \"Switch\" should be spaced correctly";

        public override IEnumerable<CodeMarkerGroup> CodeMarkerGroups
        {
            get
            {
                foreach (var language in new[] { LanguageNames.VisualBasic})
                {
                    yield return CodeMarkerGroup.Define(
                        language,
                        WarningId,
                        CodeMarkerAppearance.Warning,
                        Description,
                        true,
                        MarkerText,
                        FixText);
                }
            }
        }

        protected override void AddCodeMarkers(FileModel fileModel)
        {
            var needWarning = false;
          
            foreach (ISwitchStatement item in fileModel.All<ISwitchStatement>().Where(v => v.ExistsTextuallyInFile))
            {
                List<string> keywordSearch = new List<string> { "switch" };

                foreach (var key in keywordSearch)
                {
                    if (item.Text.WholeWordIndexOf(key) != -1)
                    {
                        needWarning = this.CheckSpacingAroundKeyword(key, item.Text);
                        if (needWarning == true)
                        {
                            item.AddCodeMarker(WarningId, this, FixSpacingAroundKeywordSwitch, item);
                            break;
                        }
                    }
                }
            }
        }

        private void FixSpacingAroundKeywordSwitch(ISwitchStatement item)
        {
            List<string> keywordSearch = new List<string> { "switch" };
            foreach (var key in keywordSearch)
            {
                item.Text = this.whiteSpaceHelper.RemoveWhiteSpaceAroundKeyword(item.Text, key);
            }
        }

        private bool CheckSpacingAroundKeyword(string key, string item)
        {
            return whiteSpaceHelper.CheckWhiteSpaceAroundKeyword(item, key);
        }
    }
}
