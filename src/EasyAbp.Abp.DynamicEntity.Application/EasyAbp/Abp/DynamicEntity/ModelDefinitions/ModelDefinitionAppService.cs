﻿using System;
using System.Linq;
using System.Threading.Tasks;
using EasyAbp.Abp.DynamicEntity.FieldDefinitions;
using EasyAbp.Abp.DynamicEntity.ModelDefinitions.Dtos;
using EasyAbp.Abp.DynamicEntity.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace EasyAbp.Abp.DynamicEntity.ModelDefinitions
{
    public class ModelDefinitionAppService : CrudAppService<ModelDefinition, ModelDefinitionDto, Guid, GetModelDefinitionListInput, CreateModelDefinitionDto, UpdateModelDefinitionDto>, IModelDefinitionAppService
    {
        protected override string GetPolicyName { get; set; } = DynamicEntityPermissions.ModelDefinition.Default;
        protected override string GetListPolicyName { get; set; } = DynamicEntityPermissions.ModelDefinition.Default;
        protected override string CreatePolicyName { get; set; } = DynamicEntityPermissions.ModelDefinition.Create;
        protected override string UpdatePolicyName { get; set; } = DynamicEntityPermissions.ModelDefinition.Update;
        protected override string DeletePolicyName { get; set; } = DynamicEntityPermissions.ModelDefinition.Delete;

        private readonly IModelDefinitionRepository _modelDefinitionRepository;
        private readonly IFieldDefinitionRepository _fieldDefinitionRepository;

        public ModelDefinitionAppService(IModelDefinitionRepository modelDefinitionRepository, IFieldDefinitionRepository fieldDefinitionRepository) : base(modelDefinitionRepository)
        {
            _modelDefinitionRepository = modelDefinitionRepository;
            _fieldDefinitionRepository = fieldDefinitionRepository;
        }

        protected override async Task<IQueryable<ModelDefinition>> CreateFilteredQueryAsync(GetModelDefinitionListInput input)
        {
            if (!input.Filter.IsNullOrEmpty())
            {
                return _modelDefinitionRepository.WhereIf(!input.Filter.IsNullOrEmpty(),
                    fd => fd.Name.Contains(input.Filter) ||
                          fd.Type.Contains(input.Filter));
            }

            return await base.CreateFilteredQueryAsync(input);
        }

        protected virtual async Task SetFields(ModelDefinition modelDefinition, UpdateModelDefinitionDto createInput)
        {
            var fields = (await _fieldDefinitionRepository.GetByIds(createInput.FieldIds))
                .ToDictionary(fd => fd.Id);

            modelDefinition.Fields.Clear();
            int order = 1;
            foreach (var fieldId in createInput.FieldIds)
            {
                modelDefinition.AddField(fields[fieldId].Id, order++);
            }
        }

        protected override async Task<ModelDefinition> MapToEntityAsync(CreateModelDefinitionDto createInput)
        {
            var entity = await base.MapToEntityAsync(createInput);
            await SetFields(entity, createInput);
            return entity;
        }

        protected override async Task MapToEntityAsync(UpdateModelDefinitionDto input, ModelDefinition entity)
        {
            await base.MapToEntityAsync(input, entity);
            await SetFields(entity, input);   
        }
        
        public async Task<ModelDefinitionDto> GetByName(string name)
        {
            var entity = await _modelDefinitionRepository.FindAsync(md => md.Name == name);
            return await MapToGetOutputDtoAsync(entity);
        }

        public override async Task<ModelDefinitionDto> CreateAsync(CreateModelDefinitionDto input)
        {
            await CheckDuplicateName(input);
            return await base.CreateAsync(input);
        }

        private async Task CheckDuplicateName(CreateModelDefinitionDto input, Guid? id = null)
        {
            var existModelDefinition = await _modelDefinitionRepository.FindAsync(md => md.Name == input.Name);
            if (existModelDefinition != null && (id == null || id.Value != existModelDefinition.Id))
            {
                throw new BusinessException(DynamicEntityErrorCodes.ModelDefinitionAlreadyExists)
                {
                    Data =
                    {
                        {"Name", input.Name}
                    }
                };
            }
        }
    }
}