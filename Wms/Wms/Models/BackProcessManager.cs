namespace Wms.Models
{
    using Dapper;
    using Share.Models;
    using System;
    using System.Data;
    using Wms.Areas.Ship.ViewModels.EcAllocation;
    using Wms.Common;

    public delegate void BackgroundFunction_OutProcedureModel(BackProcessManager manager, EcAllocationSearchConditions searchConditions);
    /// <summary>
    /// 処理ステータス
    /// </summary>
    public enum StatusType : int
    {
        /// <summary>
        /// 未処理
        /// </summary>
        /// <remarks></remarks>
        Initial = -1,
        /// <summary>
        /// 正常終了
        /// </summary>
        /// <remarks></remarks>
        Complete = 0,
        /// <summary>
        /// 処理中
        /// </summary>
        /// <remarks></remarks>
        Processing = 1,
        /// <summary>
        /// 正常終了(エラーあり)
        /// </summary>
        /// <remarks></remarks>
        CompHasErr = 2,
        /// <summary>
        /// 異常終了
        /// </summary>
        /// <remarks></remarks>
        Error = 9
    }
    /// <summary>
    /// モデルの基底クラス
    /// </summary>
    public class BackProcessManager
    {
        #region プロパティ

        /// <summary>
        /// ステータス
        /// </summary>
        public int WKID { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public StatusType Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public OutProcedureModel Result { get; set; }
        #endregion

        #region "非同期開始処理"
        /// <summary>
        /// 非同期に実行する関数を実行します。
        /// </summary>
        /// <param name="arg">非同期に実行する関数の引数</param>
        /// <remarks></remarks>
        public void Begin(BackgroundFunction_OutProcedureModel func, EcAllocationSearchConditions searchConditions)
        {
            if (Status == StatusType.Initial)
            {
                //「未処理」の場合のみ

                // 処理開始
                func.BeginInvoke(this, searchConditions, new AsyncCallback(EndCallBack), null);

                // ステータスを「処理中」に！！
                Status = StatusType.Processing;
            }
        }
        #endregion

        #region "非同期終了処理"
        /// <summary>
        /// 非同期終了処理
        /// </summary>
        /// <param name="ar"></param>
        /// <remarks></remarks>
        private void EndCallBack(IAsyncResult ar)
        {
            // DbContextが生成されていたら破棄する
            if (MvcDbContext.HasCurrent)
            {
                MvcDbContext.Current.Dispose();
                System.Diagnostics.Debug.WriteLine("Dispose DbContext.");
            }
        }
        #endregion

        #region "ステータス更新"
        /// <summary>
        /// ステータス更新
        /// </summary>
        /// <param name="per">パーセント</param>
        /// <param name="msg">メッセージ</param>
        /// <remarks></remarks>
        public void UpdateStatus(int per, string msg)
        {
            var param = new DynamicParameters();
            param.Add("ODP_IN_WK_ID", "7021", DbType.String, ParameterDirection.Input);
            param.Add("ODP_IN_CD_USER", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("ODP_IN_CLS_STATUS", this.Status, DbType.Int32, ParameterDirection.Input);
            param.Add("ODP_IN_NUM_PROGRESS", per, DbType.String, ParameterDirection.Input);
            param.Add("ODP_IN_STR_MSG", msg, DbType.Int32, ParameterDirection.Input);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "ST_SETPROGRESS",
                param,
                commandType: CommandType.StoredProcedure);

        }
        #endregion

        #region "最終ステータス更新"
        /// <summary>
        /// 最終ステータス更新
        /// </summary>
        /// <param name="per">パーセント</param>
        /// <param name="msg">メッセージ</param>
        /// <remarks></remarks>
        public void UpdateFinalStatus(StatusType sts, string msg)
        {

            if (Status == StatusType.Processing)
            {
                switch (sts)
                {
                    case StatusType.Initial:
                        // ステータスの初期化は禁止

                        break;

                    case StatusType.Processing:
                        break;

                    case StatusType.Complete:
                        this.Status = sts;
                        this.UpdateStatus(100, msg);
                        break;

                    case StatusType.Error:
                        this.Status = sts;
                        this.UpdateStatus(99, msg);
                        break;

                    case StatusType.CompHasErr:
                        this.Status = sts;
                        this.UpdateStatus(100, msg);
                        break;

                    default:
                        break;
                }
            }
        }
        #endregion

    }
}