using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;
using System;
using Sitecore.Shell.Framework.Commands;

namespace Sitecore.Support
{
    [System.Serializable]
    public class CompareVersions : Command
    {
        public override void Execute(CommandContext context)
        {
            if (context.Items.Length == 1)
            {
                Item item = context.Items[0];
                UrlString str = new UrlString(UIUtil.GetUri("control:Diff"));
                str.Append("id", item.ID.ToString());
                str.Append("la", item.Language.ToString());
                str.Append("vs", item.Version.ToString());
                str.Append("wb", "1");
                SheerResponse.CheckModified(false);
                SheerResponse.ShowModalDialog(str.ToString());
            }
        }

        public override CommandState QueryState(CommandContext context)
        {
            Error.AssertObject(context, "context");
            if (context.Items.Length != 1)
            {
                return CommandState.Hidden;
            }
            Item item = context.Items[0];
            if (((item.TemplateID == TemplateIDs.Template) || (item.TemplateID == TemplateIDs.TemplateSection)) ||
                (item.TemplateID == TemplateIDs.TemplateField))
            {
                return CommandState.Hidden;
            }
            Sitecore.Data.Version[] versionNumbers = item.Versions.GetVersionNumbers(false);
            //if ((versionNumbers != null) && (versionNumbers.Length != 0))
            if ((versionNumbers != null) && (versionNumbers.Length != 0) && item.Access.CanReadLanguage())
            {
                return base.QueryState(context);
            }
            return CommandState.Disabled;
        }
    }
}

