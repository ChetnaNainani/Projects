﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace WcfServiceForInsert
{
 
    [Table("category")]
    public class Category
    {
        
            public string Id { get; set; }
        
            public string Name { get; set; }
       
            public string Code { get; set; }
            [DisplayName("Display Name")]
           
            public string DisplayName { get; set; }
       
            public string ImageUrl { get; set; }
            [Required]
            [Range(1, 50)]
            [DisplayName("Display Priority")]
          
            public int DisplayPriority { get; set; }
        
            public string MetaTitle { get; set; }
       
            public string MetaKeywords { get; set; }
        
            public string MetaDescription { get; set; }
            [DisplayName("Is Active")]
          
            public bool isActive { get; set; }
        
    }
}