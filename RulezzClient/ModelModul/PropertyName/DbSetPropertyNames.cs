﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ModelModul.PropertyName
{
    public class DbSetPropertyNames : DbSetModel<PropertyNames>
    {
        public async Task<ObservableCollection<PropertyNames>> Load(int idGroup)
        {
            List<PropertyNames> temp = await Task.Run(() =>
            {
                try
                {
                    using (StoreEntities db = new StoreEntities())
                    {
                        return db.PropertyNames.Include(p => p.PropertyValues).Where(obj => obj.IdGroup == idGroup).ToList();
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            });
            ObservableCollection<PropertyNames> list = new ObservableCollection<PropertyNames>();

            if (temp != null)
            {
                foreach (var item in temp)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public override void Add(PropertyNames obj)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.Groups != null)
                        {
                            obj.IdGroup = obj.Groups.Id;
                            obj.Groups = null;
                        }
                        obj.PropertyValues = null;

                        db.PropertyNames.Add(obj);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public override void Update(PropertyNames obj)
        {
            throw new System.NotImplementedException();
        }

        public override void Delete(int objId)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var propertyNames = db.PropertyNames.Find(objId);
                        db.Entry(propertyNames).State = EntityState.Deleted;
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}