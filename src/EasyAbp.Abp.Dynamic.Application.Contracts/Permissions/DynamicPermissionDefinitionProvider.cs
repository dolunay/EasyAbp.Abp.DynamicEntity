using EasyAbp.Abp.Dynamic.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EasyAbp.Abp.Dynamic.Permissions
{
    public class DynamicPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(DynamicPermissions.GroupName, L("Permission:Dynamic"));

            var fieldDefinitionPermission = myGroup.AddPermission(DynamicPermissions.FieldDefinition.Default, L("Permission:FieldDefinition"));
            fieldDefinitionPermission.AddChild(DynamicPermissions.FieldDefinition.Create, L("Permission:Create"));
            fieldDefinitionPermission.AddChild(DynamicPermissions.FieldDefinition.Update, L("Permission:Update"));
            fieldDefinitionPermission.AddChild(DynamicPermissions.FieldDefinition.Delete, L("Permission:Delete"));

            var modelDefinitionPermission = myGroup.AddPermission(DynamicPermissions.ModelDefinition.Default, L("Permission:ModelDefinition"));
            modelDefinitionPermission.AddChild(DynamicPermissions.ModelDefinition.Create, L("Permission:Create"));
            modelDefinitionPermission.AddChild(DynamicPermissions.ModelDefinition.Update, L("Permission:Update"));
            modelDefinitionPermission.AddChild(DynamicPermissions.ModelDefinition.Delete, L("Permission:Delete"));

            var dynamicEntityPermission = myGroup.AddPermission(DynamicPermissions.DynamicEntity.Default, L("Permission:DynamicEntity"));
            dynamicEntityPermission.AddChild(DynamicPermissions.DynamicEntity.Create, L("Permission:Create"));
            dynamicEntityPermission.AddChild(DynamicPermissions.DynamicEntity.Update, L("Permission:Update"));
            dynamicEntityPermission.AddChild(DynamicPermissions.DynamicEntity.Delete, L("Permission:Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<DynamicResource>(name);
        }
    }
}