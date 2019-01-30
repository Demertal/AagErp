using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    public class NodeStore
    {
        public NodeStore()
        {
            DeleteStore = new DelegateCommand<object>(c =>
            {
                if (MessageBox.Show("Вы уверены что хотите удалить магазин?", "Удаление", MessageBoxButton.YesNo,
                        MessageBoxImage.Question) != MessageBoxResult.Yes) return;
                try
                {
                    using (StoreEntities db = new StoreEntities())
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                var store = db.Store.Find((c as NodeStore).Store.Id);
                                db.Entry(store).State = EntityState.Deleted;
                                db.SaveChanges();
                                transaction.Commit();
                                MessageBox.Show("Магазин удален", "Успех", MessageBoxButton.OK);
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            });
        }

        public Store Store { get; set; }
        public ObservableCollection<NodeNomenclatureGroup> NomenclatureGroups { get; set; }

        public DelegateCommand<object> DeleteStore { get; }
    }

    public class NodeNomenclatureGroup
    {
        public NomenclatureGroup NomenclatureGroup { get; set; }
        public ObservableCollection<NomenclatureSubGroup> NomenclatureSubGroups { get; set; }
    }

    class ShowStructurVM : BindableBase
    {
        private readonly IUiDialogService _dialogService = new DialogService();
        private readonly ObservableCollection<NodeStore> _groups = new ObservableCollection<NodeStore>();
        private object _selectedObject;
        private ReadOnlyObservableCollection<NodeStore> _group;

        public ReadOnlyObservableCollection<NodeStore> Groups => _group;

        public ShowStructurVM()
        {
            Reload();
            DeleteStore = new DelegateCommand(() =>
            {
                if (MessageBox.Show("Вы уверены что хотите удалить магазин?", "Удаление", MessageBoxButton.YesNo,
                        MessageBoxImage.Question) != MessageBoxResult.Yes) return;
                try
                {
                    using (StoreEntities db = new StoreEntities())
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                var store = db.Store.Find((SelectedObject as NodeStore).Store.Id);
                                db.Entry(store).State = EntityState.Deleted;
                                db.SaveChanges();
                                transaction.Commit();
                                MessageBox.Show("Магазин удален", "Успех", MessageBoxButton.OK);
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                            }
                        }
                    }
                    Reload();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            });
            DeleteNomenclatureGroup = new DelegateCommand(() =>
            {
                if (MessageBox.Show("Вы уверены что хотите удалить номенклатурную группу?", "Удаление", MessageBoxButton.YesNo,
                        MessageBoxImage.Question) != MessageBoxResult.Yes) return;
                try
                {
                    using (StoreEntities db = new StoreEntities())
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                var group = db.NomenclatureGroup.Find((SelectedObject as NodeNomenclatureGroup).NomenclatureGroup.Id);
                                db.Entry(group).State = EntityState.Deleted;
                                db.SaveChanges();
                                transaction.Commit();
                                MessageBox.Show("Номенклатурная группа удалена", "Успех", MessageBoxButton.OK);
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                            }
                        }
                    }
                    Reload();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            });
            DeleteNomenclatureSubGroup = new DelegateCommand(() =>
            {
                if (MessageBox.Show("Вы уверены что хотите удалить номенклатурную подгруппу?", "Удаление", MessageBoxButton.YesNo,
                        MessageBoxImage.Question) != MessageBoxResult.Yes) return;
                try
                {
                    using (StoreEntities db = new StoreEntities())
                    {
                        using (var transaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                var group = db.NomenclatureSubGroup.Find((SelectedObject as NomenclatureSubGroup).Id);
                                db.Entry(group).State = EntityState.Deleted;
                                db.SaveChanges();
                                transaction.Commit();
                                MessageBox.Show("Номенклатурная подгруппа удалена", "Успех", MessageBoxButton.OK);
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                            }
                        }
                    }
                    Reload();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            });
            AddStore = new DelegateCommand(() =>
            {
                _dialogService.ShowDialog(DialogService.ChoiceView.AddStore, null, true, b => { });
                Reload();
            });
            AddNomenclatureGroup = new DelegateCommand(() =>
            {
                object[] param = new object[1];
                param[0] = (SelectedObject as NodeStore).Store;
                _dialogService.ShowDialog(DialogService.ChoiceView.AddNomenclatureGroup, param, true, b => { });
                Reload();
            });
            AddNomenclatureSubGroup = new DelegateCommand(() =>
            {
                object[] param = new object[1];
                param[0] = (SelectedObject as NodeNomenclatureGroup).NomenclatureGroup;
                _dialogService.ShowDialog(DialogService.ChoiceView.AddNomenclatureSubGroup, param, true, b => { });
                Reload();
            });
            UpdateStore = new DelegateCommand(() =>
            {
                object[] param = new object[1];
                param[0] = (SelectedObject as NodeStore).Store;
                _dialogService.ShowDialog(DialogService.ChoiceView.UpdateStore, param, true, b => { });
                Reload();
            });
            UpdateNomenclatureGroup = new DelegateCommand(() =>
            {
                object[] param = new object[2];
                param[0] = (SelectedObject as NodeNomenclatureGroup).NomenclatureGroup;
                using (StoreEntities db = new StoreEntities())
                {
                    param[1] = db.Store.Find((SelectedObject as NodeNomenclatureGroup).NomenclatureGroup.IdStore);
                }
                _dialogService.ShowDialog(DialogService.ChoiceView.UpdateNomenclatureGroup, param, true, b => { });
                Reload();
            });
            UpdateNomenclatureSubGroup = new DelegateCommand(() =>
            {
                object[] param = new object[2];
                param[0] = SelectedObject as NomenclatureSubGroup;
                using (StoreEntities db = new StoreEntities())
                {
                    param[1] = db.NomenclatureGroup.Find((SelectedObject as NomenclatureSubGroup).IdNomenclatureGroup);
                }
                _dialogService.ShowDialog(DialogService.ChoiceView.UpdateNomenclatureSubGroup, param, true, b => { });
                Reload();
            });
        }

        public async void Reload()
        {
            using (StoreEntities db = new StoreEntities())
            {
                _groups.Clear();
                StoreListVm store = new StoreListVm();
                await store.Load();
                foreach (var st in store.Stores)
                {
                    NodeStore nds = new NodeStore
                    {
                        Store = st,
                        NomenclatureGroups = new ObservableCollection<NodeNomenclatureGroup>()
                    };
                    NomenclatureGroupListVm ngroup = new NomenclatureGroupListVm();
                    await ngroup.Load(st.Id);
                    foreach (var ng in ngroup.NomenclatureGroups)
                    {
                        NodeNomenclatureGroup ndg = new NodeNomenclatureGroup
                        {
                            NomenclatureGroup = ng,
                            NomenclatureSubGroups = new ObservableCollection<NomenclatureSubGroup>()
                        };
                        NomenclatureSubgroupListVm nsbgroup = new NomenclatureSubgroupListVm();
                        await nsbgroup.Load(ng.Id);
                        foreach (var nsg in nsbgroup.NomenclatureSubGroups)
                        {
                            ndg.NomenclatureSubGroups.Add(nsg);
                        }
                        nds.NomenclatureGroups.Add(ndg);
                    }
                    _groups.Add(nds);
                }
                _group = new ReadOnlyObservableCollection<NodeStore>(_groups);
                RaisePropertyChanged("Groups");
            }
        }

        public object SelectedObject
        {
            get => _selectedObject;
            set
            {
                _selectedObject = value;
                RaisePropertyChanged();
            }

        }

        public DelegateCommand DeleteStore { get; }
        public DelegateCommand DeleteNomenclatureGroup { get; }
        public DelegateCommand DeleteNomenclatureSubGroup { get; }
        public DelegateCommand AddStore { get; }
        public DelegateCommand AddNomenclatureGroup { get; }
        public DelegateCommand AddNomenclatureSubGroup { get; }
        public DelegateCommand UpdateStore { get; }
        public DelegateCommand UpdateNomenclatureGroup { get; }
        public DelegateCommand UpdateNomenclatureSubGroup { get; }
    }
}
