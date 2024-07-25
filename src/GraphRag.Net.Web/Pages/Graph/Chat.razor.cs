﻿using AntDesign;
using GraphRag.Net.Domain.Interface;
using Microsoft.AspNetCore.Components;

namespace GraphRag.Net.Web.Pages.Graph
{
    public partial class Chat
    {
        [Inject] IGraphService _graphService { get; set; }
        [Inject] IMessageService _message { get; set; }
        private List<UploadFileItem> _fileList = [];
        private List<string> _indexList { get; set; }
        private bool loadding = false;
        private string _index;
        private string _input;
        private string _output;

        private string _importIndex;
        private string _importText;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _indexList = _graphService.GetAllIndex();
        }

        private async Task Search()
        {
            loadding = true;
            _output = await _graphService.SearchGraphAsync(_index, _input);
            loadding = false;
        }

        private void OnSelectedItemChangedHandler(string value)
        {
            _index = value;
        }


        private bool BeforeUpload(UploadFileItem file)
        {

            if (file.Type != "text/plain")
            {
                _message.Error("文件格式错误,请重新选择!");
            }
            var IsLt500K = file.Size < 1024 * 1024 * 100;
            if (!IsLt500K)
            {
                _message.Error("文件需不大于100MB!");
            }

            return IsLt500K;
        }

        private void OnSingleCompleted(UploadInfo fileinfo)
        {
            _indexList = _graphService.GetAllIndex();
            _message.Info("导入完成");
        }

        private async Task InputText()
        {
            await _graphService.InsertGraphDataAsync(_importIndex, _importText);
            _indexList = _graphService.GetAllIndex();
            _message.Info("导入完成");
        }
    }
}
