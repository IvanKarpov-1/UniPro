﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("ConnectionUriDomain", "AppId", "TenantId", "FactorId")]
[Table("st_tenant_first_factors")]
[Index("ConnectionUriDomain", "AppId", "TenantId", Name = "tenant_first_factors_tenant_id_index")]
public partial class StTenantFirstFactor
{
    [Key]
    [Column("connection_uri_domain")]
    [StringLength(256)]
    public string ConnectionUriDomain { get; set; } = null!;

    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("tenant_id")]
    [StringLength(64)]
    public string TenantId { get; set; } = null!;

    [Key]
    [Column("factor_id")]
    [StringLength(128)]
    public string FactorId { get; set; } = null!;

    [ForeignKey("ConnectionUriDomain, AppId, TenantId")]
    [InverseProperty("StTenantFirstFactors")]
    public virtual StTenantConfig StTenantConfig { get; set; } = null!;
}
