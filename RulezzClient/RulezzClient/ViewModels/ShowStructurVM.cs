//using System;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Windows;
//using Prism.Commands;
//using Prism.Mvvm;

//namespace RulezzClient.ViewModels
//{
//    public class Nodes
//    {
//        public Groups Group { get; set; }
//        public ObservableCollection<Nodes> Node { get; set; }
//    }

//    class ShowStructurVM : BindableBase
//    {
//        #region Parametrs

//        private readonly IUiDialogService _dialogService = new DialogService();
//        private readonly ObservableCollection<Nodes> _groups = new ObservableCollection<Nodes>();
//        private Nodes _selectedNode;
//        private ReadOnlyObservableCollection<Nodes> _group;

//        public ReadOnlyObservableCollection<Nodes> Groups => _group;

//        #endregion

//        public ShowStructurVM()
//        {
//            Reload();
//            DeleteNode = new DelegateCommand(() =>
//            {
//                if (MessageBox.Show("Вы уверены что хотите удалить номенклатурную группу?", "Удаление", MessageBoxButton.YesNo,
//                        MessageBoxImage.Question) != MessageBoxResult.Yes) return;
//                try
//                {
//                    using (StoreEntities db = new StoreEntities())
//                    {
//                        using (var transaction = db.Database.BeginTransaction())
//                        {
//                            try
//                            {
//                                //var group = db.NomenclatureGroup.Find((SelectedNode as NodeNomenclatureGroup).NomenclatureGroup.Id);
//                                //db.Entry(group).State = EntityState.Deleted;
//                                db.SaveChanges();
//                                transaction.Commit();
//                                MessageBox.Show("Номенклатурная группа удалена", "Успех", MessageBoxButton.OK);
//                            }
//                            catch (Exception ex)
//                            {
//                                transaction.Rollback();
//                                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
//                                    MessageBoxImage.Error);
//                            }
//                        }
//                    }
//                    Reload();
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
//                        MessageBoxImage.Error);
//                }
//            });
//            AddNode = new DelegateCommand(() =>
//            {
//                object[] param = new object[1];
//                if(SelectedNode.GetType().FullName == "RulezzClient.ViewModels.Nodes")
//                    param[0] = SelectedNode.Group;
//                else
//                {
//                    Groups group;
//                    using (StoreEntities db = new StoreEntities())
//                    {
//                        string title = SelectedNode.Group.Title;
//                        group = db.Groups.FirstOrDefault(gr => gr.Id == gr.IdParentGroup && gr.Title == title);
//                    }
//                    param[0] = group;
//                }
//                _dialogService.ShowDialog(DialogService.ChoiceView.AddNode, param, true, b => { });
//                Reload();
//            });
//            UpdateNode = new DelegateCommand(() =>
//            {
//                //object[] param = new object[2];
//                //param[0] = (SelectedNode as NodeNomenclatureGroup).NomenclatureGroup;
//                //using (StoreEntities db = new StoreEntities())
//                //{
//                //    param[1] = db.Store.Find((SelectedNode as NodeNomenclatureGroup).NomenclatureGroup.IdStore);
//                //}
//                //_dialogService.ShowDialog(DialogService.ChoiceView.UpdateNomenclatureGroup, param, true, b => { });
//                //Reload();
//            });
//        }

//        public async void Reload()
//        {
//            _groups.Clear();
//            GroupListVm group = new GroupListVm();
//            await group.LoadParent();
//            foreach (var gr in group.Groups)
//            {
//                Nodes nds = new Nodes
//                {
//                    Group = gr,
//                    Node = await LoadNode(gr.Id)
//                };
//                _groups.Add(nds);
//            }
//            _group = new ReadOnlyObservableCollection<Nodes>(_groups);
//            RaisePropertyChanged("Groups");
//        }

//        public async Task<ObservableCollection<Nodes>> LoadNode(int idParent)
//        {
//            ObservableCollection<Nodes> observ = new ObservableCollection<Nodes>();
//            GroupListVm group = new GroupListVm();
//            await group.Load(idParent);
//            foreach (var item in group.Groups)
//            {
//                if(item.Id == item.IdParentGroup) continue;
//                Nodes nd = new Nodes
//                {
//                    Group = item,
//                    Node = await LoadNode(item.Id)
//                };
//                observ.Add(nd);
//            }
//            return observ;
//        }

//        public Nodes SelectedNode
//        {
//            get => _selectedNode;
//            set
//            {
//                _selectedNode = value;
//                RaisePropertyChanged();
//            }

//        }

//        public DelegateCommand DeleteStore { get; }
//        public DelegateCommand DeleteNode { get; }
//        public DelegateCommand AddStore { get; }
//        public DelegateCommand AddNode { get; }
//        public DelegateCommand UpdateStore { get; }
//        public DelegateCommand UpdateNode { get; }
//    }
//}
