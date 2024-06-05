using Volo.Abp.Settings;

namespace CrmApp.Settings;

public class CrmAppSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(CrmAppSettings.MySetting1));
    }
}
