using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Wms.Areas.Master.ViewModels.EditLayout;
using Wms.Common;

namespace Wms.Areas.Master.Models.EditLayout
{
    public class EditImportModel
    {
        /// <summary>
        /// <para>Streamからテキスト取得</para>
        /// <para>ヘッダーと内容で2行決め打ち</para>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="headingRow"></param>
        /// <returns></returns>
        private static (string, string) GetHeaderAndBodyText(StreamReader reader, HeadingRow headingRow)
        {
            string head, body;
            if (headingRow == HeadingRow.Available)
            {
                head = reader.ReadLine();
                body = reader.ReadLine();
            }
            else
            {
                head = "";
                body = reader.ReadLine();
            }
            return (head, body);
        }

        /// <summary>
        /// <para>テキストを分割して返す</para>
        /// <para>セパレータは<see cref="FileType">fileType</see>で判別する</para>
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        private static List<string> GetItemsWithFileType(string text, FileTypeImport fileType)
        {
            if (fileType == FileTypeImport.CSV)
            {
                return text.Split(',').ToList();
            }
            else
            {
                return text.Split('\t').ToList();
            }
        }

        private static List<string> GetHeaderWithFileType(string text, int itemCount, FileTypeImport fileType)
        {
            var heads = new List<string>();
            if (string.IsNullOrEmpty(text))
            {
                for (int i = 1; i <= itemCount; i++)
                {
                    heads.Add("項目" + i);
                }
                return heads;
            }

            if (fileType == FileTypeImport.CSV)
            {
                heads = text.Split(',').ToList();
            }
            else
            {
                heads = text.Split('\t').ToList();
            }

            if (heads.Count != itemCount)
            {
                return null;

            }
            return heads;
        }

        /// <summary>
        /// ファイルから内容取得して<see cref="EditImport">インスタンスを返却
        /// </summary>
        /// <param name="file"></param>
        /// <param name="encodeType"></param>
        /// <param name="fileType"></param>
        /// <param name="headingRow"></param>
        /// <returns></returns>
        public static EditImport InstanceFromDroppedFile(HttpPostedFileBase file, EncodeType encodeType, FileTypeImport fileType, HeadingRow headingRow)
        {
            // StreamReaderインスタンス作成、
            // エンコードタイプによって読み取るエンコードを判別
            using (var text = new StreamReader(file.InputStream,
                encodeType == EncodeType.ShiftJis ?
                    System.Text.Encoding.GetEncoding(932)
                    : System.Text.Encoding.UTF8))
            {
                // ファイルからヘッダーと内容の取得
                (string head, string body) = GetHeaderAndBodyText(text, headingRow);

                // 内容がない場合はnull返却
                if (string.IsNullOrEmpty(body))
                {
                    return null;
                }

                // 内容をそれぞれの要素に分割、セパレータはファイル区分で判別
                var items = GetItemsWithFileType(body, fileType);

                // ヘッダーを取得
                var heads = GetHeaderWithFileType(head, items.Count, fileType);

                // ヘッダーと内容を元に、EditImportFileInfoのリストを作成し返却する
                // EditImportインスタンスに入れて返す。
                var vm = new EditImport();
                vm.FileInfos = new List<EditImportFileInfo>();
                for (int i = 0; i < items.Count; i++)
                {
                    vm.FileInfos.Add(new EditImportFileInfo { Title = heads[i], DataDetail = items[i] });
                }

                return vm;
            }

        }
    }
}