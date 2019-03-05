using MasterChief.DotNet.Core.Contract;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterChief.DotNet.Core.EFTests
{
    [Table("EFSample")]
    [Description("EF 测试表")]
    public sealed class EFSample : ModelBase
    {
        [Key]
        public override Guid ID { get => base.ID; set => base.ID = value; }

        public EFSample()
        {
            ID = Guid.NewGuid();
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;
            Available = true;
        }

        [MaxLength(20)]
        [Display(Name = "User Name")]
        public string UserName { get; set; } // nvarchar(20), null
    }
}