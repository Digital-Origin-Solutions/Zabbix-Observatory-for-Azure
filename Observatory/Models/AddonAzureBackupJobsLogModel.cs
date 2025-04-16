namespace Observatory.Models
{
    internal class AddonAzureBackupJobsLogModel
    {
        public required string JobUniqueId { get; set; }
        public DateTimeOffset? TimeGenerated { get; set; }
        public string? TenantId { get; set; }
        public string? SourceSystem { get; set; }
        public string? ResourceId { get; set; }
        public string? OperationName { get; set; }
        public string? Category { get; set; }
        public string? AdHocOrScheduledJob { get; set; }
        public string? BackupItemUniqueId { get; set; }
        public string? TargetName { get; set; }
        public string? Region { get; set; }
        public string? BackupManagementServerUniqueId { get; set; }
        public string? BackupManagementType { get; set; }
        public double? DataTransferredInMB { get; set; }
        public double? JobDurationInSecs { get; set; }
        public string? JobFailureCode { get; set; }
        public string? JobOperation {  get; set; }
        public string? JobOperationSubType { get; set; }
        public DateTimeOffset? JobStartDateTime { get; set; }
        public string? JobStatus { get; set; }
        public string? ProtectedContainerUniqueId { get; set; }
        public string? RecoveryJobDestination { get; set; }
        public string? RecoveryJobRPDateTime { get; set; }
        public string? RecoveryJobRPLocation { get; set; }
        public string? RecoveryLocationType { get; set; }
        public string? SchemaVersion { get; set; }
        public string? State { get; set; }
        public string? VaultUniqueId { get; set; }
        public string? DatasourceSetFriendlyName { get; set; }
        public string? DatasourceSetResourceId { get; set; }
        public string? DatasourceSetType { get; set; }
        public string? DatasourceResourceId { get; set; }
        public string? DatasourceType { get; set; }
        public string? DatasourceFriendlyName { get; set; }
        public string? SubscriptionId { get; set; }
        public string? ResourceGroupName { get; set; }
        public string? VaultName { get; set; }
        public string? VaultTags { get; set; }
        public string? VaultType { get; set; }
        public string? StorageReplicationType { get; set; }
        public string? ArchiveTierStorageReplicationType { get; set; }
        public string? AzureDataCenter { get; set; }
        public string? BackupItemId { get; set; }
        public string? BackupItemFriendlyName { get; set; }
        public string? ExtendedProperties { get; set; }
        public string? Type { get; set; }
        public string? _ResourceId { get; set; }
    }
}
