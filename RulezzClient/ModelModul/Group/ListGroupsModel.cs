using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace ModelModul.Group
{
    public class ListGroupsModel : BindableBase
    {
        #region Properties

        private GroupModel _parentGroup;

        public GroupModel ParentGroup
        {
            get => _parentGroup;
            set
            {
                _parentGroup = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<ListGroupsModel> _groups = new ObservableCollection<ListGroupsModel>();

        public ObservableCollection<ListGroupsModel> Groups
        {
            get => _groups;
            set => SetProperty(ref _groups, value);
        }

        #endregion

        #region Сonstructors

        public ListGroupsModel()
        {
            Groups = new ObservableCollection<ListGroupsModel>();
            ParentGroup = null;
        }

        public ListGroupsModel(GroupModel parentGroup)
        {
            Groups = new ObservableCollection<ListGroupsModel>();
            ParentGroup = parentGroup;
        }

        #endregion

        public async Task<int> Load()
        {
            bool Pre(Groups obj)
            {
                if (obj.IdParentGroup == null && ParentGroup == null) return true;
                return obj.IdParentGroup != null && ParentGroup != null && obj.IdParentGroup.Value == ParentGroup.Id;
            }

            List<ListGroupsModel> temp = await Task.Run(() =>
            {
                using (StoreEntities db = new StoreEntities())
                {
                    return db.Groups.Where(Pre).Select(obj => new ListGroupsModel(new GroupModel
                    {

                        Id = obj.Id,
                        Title = obj.Title,
                        IdParentGroup = obj.IdParentGroup
                    })).ToList();
                }
            });

            ObservableCollection<ListGroupsModel> group = new ObservableCollection<ListGroupsModel>();

            if (temp != null)
            {
                foreach (var item in temp)
                {
                    group.Add(item);
                }
            }

            Groups = group;
            return Groups.Count;
        }

        public void Add(GroupModel group)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Groups.Add(group.ConvertToGroups());
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public bool Delete(int id)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var group = db.Groups.Find(id);
                        db.Entry(group).State = EntityState.Deleted;
                        db.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public void Update(GroupModel group)
        {
            using (StoreEntities db = new StoreEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var modifi = db.Groups.Find(group.Id);
                        if (modifi != null)
                        {
                            modifi.Title = group.Title;
                            db.Entry(modifi).State = EntityState.Modified;
                        }
                        else throw new Exception("Изменение не удалось");
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
