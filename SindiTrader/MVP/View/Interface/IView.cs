//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   IView.cs
//! @brief  
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


//
using System;

namespace SindiTrader
{
    //
    public interface IView : IPresenterToView
    {
        void AddAction(IViewToPresenter ivtp);
    }


    //
    public interface IViewToPresenter
    {
        void Init(string id,/*신한인디아이디*/ string pw,/*신한인디비번*/ string certificate,/*공인인증비번*/ string exe/*신한인디 실행파일 경로*/, string 단축코드);
        void Close();
        void Request(string 단축코드);
    }


}// namespace SindiTrader
