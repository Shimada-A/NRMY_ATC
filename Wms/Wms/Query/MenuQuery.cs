namespace Wms.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Dapper;
    using Wms.ViewModels;

    public partial class Menu
    {
        /// <summary>
        /// メニューの一覧を取得します。
        /// </summary>
        /// <param name="shipperId">荷主ID</param>
        /// <param name="userId">ユーザーID</param>
        /// <returns></returns>
        public List<MenuProgram> Listing()
        {
            var sql = @"
                SELECT
                   ROWNUM ROW_ID
                  ,M.*
                  ,P.*
                FROM
                    M_MENUS M
                LEFT JOIN
                    M_PROGRAMS P
                ON  M.SHIPPER_ID = P.SHIPPER_ID
                AND M.PROGRAM_ID = P.PROGRAM_ID
                LEFT JOIN
                    M_USER_PROGRAMS U
                ON  P.SHIPPER_ID = U.SHIPPER_ID
                AND P.PROGRAM_ID = U.PROGRAM_ID
                AND U.USABLE_CLASS > 0
                WHERE
                    M.SHIPPER_ID = :SHIPPER_ID
                 AND (M.PARENT_ID is null or :PERMISSION_LEVEL = U.PERMISSION_LEVEL)
                 AND M.PROGRAM_CLASS= 1
                ORDER BY M.SORT_NO
            ";

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(new
            {
                SHIPPER_ID = Common.Profile.User.ShipperId,
                PERMISSION_LEVEL = Common.Profile.User.PermissionLevel,
            });

            return MvcDbContext.Current.Database.Connection.Query<Menu, Program, MenuProgram>(
                sql,
                (menu, program) =>
                {
                    var menuProgram = new MenuProgram
                    {
                        Menu = menu,
                        Program = program
                    };
                    return menuProgram;
                },
                param: parameters,
                splitOn: "MAKE_DATE,MAKE_DATE")
                .ToList();
        }
    }
}