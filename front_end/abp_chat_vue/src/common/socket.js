import UTILS from './util';
let url = 'ws://localhost:5000/abpChatHub?access_token=' + UTILS.getUserToken();
let ws = null;
let isConnect = false;
//用来控制重连方法执行玩了才进行下一次重连
let lock = false;
//方法字典，给接受的数据进行调用
let methodDic = {
    //心跳，分成三个类型，warning,exception,info
    "heartBeat": (msg) => {
        console.log(msg);
    }
};
// init();
//创建websocket
function init(response) {
    // 打开一个 web socket
    //如果已经连接不应该再次创建连接
    if (isConnect) {
        return;
    }
    try {
        ws = new WebSocket(url);
        connect(response);
    } catch (e) {
        //如果出错了就要重连
        reconnect();
    }
}
//连接固定步骤
function connect(response) {
    //要保证连接
    ws.onopen = () => {
        // Web Socket 已连接上，使用 send() 方法发送数据
        ws.send(`{"protocol":"json", "version":1}${String.fromCharCode(0x1e)}`);
        console.log('数据发送中...');
        //重置心跳
        heartCheck.start();
    };
    ws.onmessage = (evt) => {
        //重置心跳
        heartCheck.start();
        var received_msg = evt.data.split(String.fromCharCode(0x1e));
        received_msg.forEach(data => {
            if (data === '{}' || data === '' || data.length === 0) {
                isConnect = true;
                return;
            }
            let msg = JSON.parse(data);
            //这里只能根据特定情况做特定处理
            if (msg.type === 1) {
                //主页面信息交互回调
                response(msg);
                if (msg.target !== "heartBeat") {
                    return;
                }
                //根据方法字典调用方法
                methodDic[msg.target](...msg.arguments);
            }
        });
    };
    ws.onerror = () => {
        console.log("socket连接失败,尝试重连");
        isConnect = false;
        reconnect();
    };
    ws.onclose = () => {
        // 关闭 websocket是要尝试重连
        console.log("连接已关闭...,尝试重连");
        isConnect = false;
        reconnect();
    };
}
//重连
function reconnect() {
    //如果没有成功连接才进行重连,并且锁是开着的
    console.log(lock ? '锁住了，不执行重连' : '没有锁，开始执行重连');
    if (!isConnect && !lock) {
        lock = true;
        setTimeout(() => {
            init();
            //执行完之后要把锁打开
            lock = false;
        }, 2000);
    }
}
//发送消息
function sendMessage() {
    //连接状态才能发送消息
    if (isConnect) {
        //发送数据
        ws.send(
            `${JSON.stringify({ "arguments": [user, message], "invocationId": "0", "streamIds": [], "target": "SendMessage", "type": 1 })}${String.fromCharCode(0x1e)}`
        );
    }
}
var heartCheck = {
    //每隔几秒测试一下心跳是否在继续
    timeout: 5000,
    timeoutObj: null,
    serverTimeoutObj: null,
    start: function () {
        var self = this;
        this.timeoutObj && clearTimeout(this.timeoutObj);
        this.serverTimeoutObj && clearTimeout(this.serverTimeoutObj);
        this.timeoutObj = setTimeout(function () {
            ws.send(
                `${JSON.stringify({ "arguments": ['心跳'], "invocationId": "0", "streamIds": [], "target": "Hearbeat", "type": 1 })}${String.fromCharCode(0x1e)}`
            );
            self.serverTimeoutObj = setTimeout(function () {
                console.log("后台挂掉，没有心跳了....");
                ws.close();
            }, self.timeout);

        }, this.timeout)
    }
};




//监听窗口关闭事件，当窗口关闭时，主动去关闭webSocket连接，防止连接还没断开就关闭窗口，server端会抛异常
window.onbeforeunload = () => {
    ws.onclose();
};

export { init };