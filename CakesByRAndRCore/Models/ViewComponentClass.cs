using CakesByRAndRCore.Data;
using CakesByRAndRCore.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security;

namespace CakesByRAndRCore.Models
{
    public class ViewComponentClass
    {
    }

    public class PublicMenuViewComponent : ViewComponent
    {
        private readonly CakesByRAndRCoreContext myContext;


        public PublicMenuViewComponent(CakesByRAndRCoreContext context)
        {
            myContext = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            UnitOfWork unitOfWork = new UnitOfWork(myContext);

            //UserPermission permission = new UserPermission();
            ////permission.Menu = unitOfWork.GetCakeCategories($"EXEC usp_GetCakeCategory_ParentItems").ToList();

            List<CakeCategory> publicMenu = unitOfWork.GetCakeCategories($"EXEC usp_GetCakeCategory_ParentItems").ToList();


            return View("PublicMenu", publicMenu);


        }

        //private Task<List<TodoItem>> GetItemsAsync(int maxPriority, bool isDone)
        //{
        //    return db.ToDo.Where(x => x.IsDone == isDone &&
        //                         x.Priority <= maxPriority).ToListAsync();
        //}
    }

    public class AdminMenuViewComponent : ViewComponent
    {
        private readonly CakesByRAndRCoreContext myContext;


        public AdminMenuViewComponent(CakesByRAndRCoreContext context)
        {
            myContext = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            UnitOfWork unitOfWork = new UnitOfWork(myContext);
            AdminMenu adminMenu = new AdminMenu();

       
            adminMenu.Menu = unitOfWork.GetRecordSet<ParentMenu>($"EXEC usp_GetAdminTask_ParentItems 1").ToList();
            foreach (var item in adminMenu.Menu)
            {
                item.ChildMenus = unitOfWork.GetRecordSet<ParentMenu>($"EXEC usp_GetAdminTask_ChildItems {item.Id},1").ToList();
                foreach (var itemC in item.ChildMenus)
                {
                    itemC.ChildMenus = unitOfWork.GetRecordSet<ParentMenu>($"EXEC usp_GetAdminTask_ChildItems {itemC.Id},1").ToList();
                    foreach (var itemD in itemC.ChildMenus)
                    {
                        itemD.ChildMenus = unitOfWork.GetRecordSet<ParentMenu>($"EXEC usp_GetAdminTask_ChildItems {itemD.Id},1").ToList();
                    }
                }
            }
            return View("AdminMenu", adminMenu);



            //permission.Menu = unitOfWork.GetCakeCategories($"EXEC usp_GetCakeCategory_ParentItems").ToList();
            //List <CakeCategory> publicMenu = unitOfWork.GetCakeCategories($"EXEC usp_GetCakeCategory_ParentItems").ToList();




        }

        //private Task<List<TodoItem>> GetItemsAsync(int maxPriority, bool isDone)
        //{
        //    return db.ToDo.Where(x => x.IsDone == isDone &&
        //                         x.Priority <= maxPriority).ToListAsync();
        //}
    }


}
