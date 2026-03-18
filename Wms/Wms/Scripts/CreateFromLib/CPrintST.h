/*************************************************************************
 *	CPrintST.h
 *		for CPrintST.dll
 *************************************************************************/
#ifndef CPRINTST_H
#define CPRINTST_H

#ifdef __cplusplus
extern "C" {
#endif

/*************************************************************************/
#define DllImport				__declspec( dllimport )

/* 進行ゲージ */
#define CPW_HIDE				0x00000001L		/* 非表示 */
#define CPW_MINIMIZE			0x00000002L		/* 最小化表示 */
#define CPW_SHOW				0x00000003L		/* 標準表示 */

/* エラー確認ダイアログ */
#define ERW_SHOW				0x00000100L		/* 表示 */
#define ERW_HIDE				0x00000200L		/* 非表示 */

/*************************************************************************/
typedef int ( WINAPI *PFNCPRINTST ) ( HWND, UINT, LPSTR, LPSTR );
typedef int ( WINAPI *PFNCPRINTSTV ) ( HWND, UINT, LPSTR, LPSTR, ... );
typedef int ( WINAPI *PFNCPRINTSTS ) ( LPSTR );
typedef int ( WINAPI *PFNCPCOMPRESS ) ( HWND, UINT, LPSTR, LPSTR );


/*************************************************************************
 *	int CPrintST ( HWND hWnd, UINT uCmdShow, LPSTR lpTitle, LPSTR lpCommand )
 *		引数	hWnd		: オーナーアプリケーションのウィンドウハンドル
 *				uCmdShow	: ウィンドウの表示スタイル(注1)
 *				lpTitle		: タイトル文字列
 *				lpCommand	: 実行引数文字列
 *		戻り値	int			: 1  正常終了
 *							  !1 異常終了
 *************************************************************************/
DllImport int WINAPI CPrintST ( HWND, UINT, LPSTR, LPSTR );

/*************************************************************************
 *	int CPrintSTV ( HWND hWnd, UINT uCmdShow, LPSTR lpTitle, LPSTR lpCommandV, ... )
 *		引数	hWnd		: オーナーアプリケーションのウィンドウハンドル
 *				uCmdShow	: ウィンドウの表示スタイル(注1)
 *				lpTitle		: タイトル文字列
 *				lpCommandV	: 実行引数文字列（可変個数）
 *		戻り値	int			: 1  正常終了
 *							  !1 異常終了
 *************************************************************************/
DllImport int WINAPI CPrintSTV ( HWND, UINT, LPSTR, LPSTR, ... );

/*************************************************************************
 *	int CPrintSTS ( LPSTR lpCommand )
 *		引数	lpCommand	: 実行引数文字列
 *		戻り値	int			: 1  正常終了
 *							  !1 異常終了
 *************************************************************************/
DllImport int WINAPI CPrintSTS ( LPSTR );

/*************************************************************************
 *	int CPCompress ( HWND hWnd, UINT uCmdShow, LPSTR lpTitle, LPSTR lpCommand )
 *		引数	hWnd		: オーナーアプリケーションのウィンドウハンドル
 *				uCmdShow	: ウィンドウの表示スタイル(注1)
 *				lpTitle		: タイトル文字列
 *				lpCommand	: 実行引数文字列
 *		戻り値	int			: 1  正常終了
 *							  !1 異常終了
 *************************************************************************/
DllImport int WINAPI CPCompress ( HWND, UINT, LPSTR, LPSTR );

/*************************************************************************
 *	注1 ウィンドウの表示スタイルについて
 *		CPrintSTとCPrintSTVでは、引数uCmndShowでCreate!Form PrintStageの
 *		進行状況ウィンドウの表示状態やエラー発生時の処理を制御することが
 *		できます。
 *		以下の値の組み合わせで指定します。
 *		■進行状況ウィンドウ
 *			CPW_SHOW		進行状況ウィンドウを画面中央に表示する。
 *			CPW_HIDE(*)		進行状況ウィンドウを表示しない。
 *			CPW_MINIMIZE	進行状況ウィンドウを最小化して表示する。
 *		これら以外の値を指定した場合は、CPW_SHOWとして処理されます。
 *		■エラー確認ダイアログ
 *			ERW_SHOW		エラー発生時にメッセージボックスを表示する。
 *			ERW_HIDE(*)		エラーが発生してもメッセージボックスを表示しない。
 *		これら以外の値を指定した場合は、ERW_SHOWとして処理されます。
 *		それぞれの項目について指定しなかった場合は、各(*)の処理状態が
 *		使用されます。
 *************************************************************************/


#ifdef __cplusplus
}
#endif

#endif	/* CPRINTST_H */
/* End of CPrintST.h *******************************************************/
