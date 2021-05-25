﻿using ExchangeServer.SQLDataBase;
using ExchangeSystem.Requests.Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExchangeServer.MVC.Models
{
    public class SourceModel
    {
        /// <summary>
        /// Получает весь контент по заданной коллекции id или Null
        /// </summary>
        /// <returns>Null - exception</returns>
        public Source[] GetAllOrDefault(int[] ids)
        {
            try
            {
                using (SourcesDbContext db = new SourcesDbContext())
                {
                    List<Source> findSource = new List<Source>();
                    foreach (var id in ids)
                    {
                        var findOne = db.Sources.Where(src => src.Id == id).FirstOrDefault();
                        if (findOne != null)
                            findSource.Add(findOne);
                    }
                    return findSource.ToArray();
                }
            }
            catch { return null; }
        }
    }
}
