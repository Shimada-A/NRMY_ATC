namespace Wms.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Wms.ViewModels;

    /// <summary>
    /// Menuのヘルパーメソッド
    /// </summary>
    public static class MenuHelper
    {
        /// <summary>
        /// ツリーメニューのHTMLを返します。
        /// </summary>
        /// <param name="helper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="menus">メニューのノードリスト。</param>
        /// <returns>ツリーメニューのHTML</returns>
        public static IHtmlString MenuTree(this HtmlHelper helper, IEnumerable<MenuProgram> menus)
        {
            // ルートノードを起点にメニューツリーを組み立てる
            var roots = menus.Where(m => m.Menu.ParentId == null).OrderBy(m => m.Menu.SortNo);

            var html = new StringBuilder();
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            html.AppendLine($"<ul class='nav-drawer'>");
            html.AppendLine($" <li class='nav-item'> <a href='{urlHelper.Action("Index", "Home", new { area = string.Empty })}'><i class='mdi mdi-home'></i> ダッシュボード</a> </li>  ");
            foreach (var r in roots)
            {
                html.Append(CreateNode(menus, r.Menu.ProgramId));
            }

            html.AppendLine($"</ul>");

            return MvcHtmlString.Create(html.ToString());
        }

        /// <summary>
        /// メニューの1Node分のHTMLを返します。
        /// </summary>
        /// <param name="menus">メニューリスト</param>
        /// <param name="nodeId">対象のNode</param>
        /// <returns>メニューの1Node分のHTML</returns>
        private static IHtmlString CreateNode(IEnumerable<MenuProgram> menus, string nodeId)
        {
            var html = new StringBuilder();
            var children = menus.Where(m => m.Menu.ParentId == nodeId).OrderBy(m => m.Menu.SortNo);
            var menu = menus.Where(m => m.Menu.ProgramId == nodeId).SingleOrDefault();

            if (children.Any())
            {
                // 親ノード
                html.AppendLine($"<li class='nav-item nav-item-has-subnav'>");
                html.AppendLine($"     <a href='javascript:void(0)'><i class='mdi {menu.Menu.Icon}'></i>{menu.Menu.MenuName}</a>");
                html.AppendLine($"     <ul class='nav-subnav'>");
                foreach (var c in children)
                {
                    html.Append(CreateNode(menus, c.Menu.ProgramId));
                }

                html.AppendLine($"     </ul>");
                html.AppendLine($" </li>");
            }
            else
            {
                // 子ノード
                var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

                if (menu.Program != null)
                {
                    html.AppendLine($"  <li> <a  class='mdi {menu.Menu.Icon}' href='{urlHelper.Action(menu.Program.ActionName, menu.Program.ControllerName, new { area = menu.Program.AreaName ?? string.Empty })}'>{menu.Program.ProgramName}</a> </li>");
                }
            }

            return MvcHtmlString.Create(html.ToString());
        }

        /// <summary>
        /// 指定したノードIDのレベル(階層の深さ)を取得する。
        /// </summary>
        /// <param name="targertNodeId">レベルを調べるノードID</param>
        /// <param name="menus">レベルを調べる対象のMenuProgramコレクション</param>
        /// <returns>ノードレベル</returns>
        /// <remarks>
        /// 自身からルートに向かって再帰的にレベルを計算する。
        /// ルートのレベルは0。
        /// </remarks>
        static int GetNodeLevel(string targertNodeId, IEnumerable<MenuProgram> menus)
        {
            var parentCount = 0;

            // 注：子ID(ChildId)が自身のID
            var parentId = menus.Where(m => m.Menu.ProgramId == targertNodeId).Select(m => m.Menu.ParentId).FirstOrDefault();
            if (string.IsNullOrEmpty(parentId) == false)
            {
                parentCount++;
                parentCount += GetNodeLevel(parentId, menus);
            }

            return parentCount;
        }

        /// <summary>
        /// 表示値を生成する。
        /// </summary>
        /// <param name="menuValue">メニュー値</param>
        /// <param name="nodeLevel">ノードレベル</param>
        /// <returns>表示する文字列</returns>
        static string CreatDisplayValue(string menuValue, int nodeLevel)
        {
            // 2階層レベル(見た目的には3階層目)までは標準でインデントがかかっているため、以降分インデントをかける。
            var count = nodeLevel - 2;
            if (count < 0) count = 0;

            // coun分スペースを加えたメニュー値を返す。
            return string.Concat(Enumerable.Repeat(@"&nbsp;&nbsp;", count)) + menuValue;
        }
    }
}