﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LsWebsocketClient;
using Mapster;
using MapsterMapper;
using RW.Position.Common;
using RW.Position.Models;

namespace RW.Position
{
    public class OnDemandSubscription
    {
        private readonly LocalsenseInterface _client;
        private readonly IMapper _mapper;
        private readonly IFreeSql _freeSql;

        public OnDemandSubscription() {
            var setting = new CommonSetting("", 1000, "", "");
            _client = new LocalsenseInterface(setting.IP, setting.Port, setting.UserName, setting.Password, setting.Salt);
            _client.OnMessagePos += (sender, e) =>
                OnHandlerPos(e.Data);
        }

        public OnDemandSubscription(IMapper mapper, IFreeSql freeSql):base()
        {
            _mapper = mapper;
            _freeSql = freeSql;
            
        }

        /// <summary>
        /// 标签信息回调
        /// </summary>
        /// <param name="data"></param>
        public void OnHandlerPos(List<LsPosInfo> data)
        {
            var addModel = _mapper.Map<List<PositionInfo>>(data);
            _freeSql.Insert(addModel).ExecuteAffrows();
        }
    }
}
