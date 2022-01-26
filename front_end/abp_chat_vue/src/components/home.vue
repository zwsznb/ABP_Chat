<template>
  <div class="chat-box">
    <el-container class="common-border">
      <el-aside width="300px" class="custom-flex">
        <div class="aside-left flex1">
          <img alt="header" class="header margin-top30" src="../assets/static/header.jpg" />
          <div class="custom-flex-column">
            <i class="el-icon-chat-round chat-icon"></i>
            <el-badge :is-dot="isExistNewInvitation" class="item">
              <i class="el-icon-message chat-icon" @click="findInvitation"></i>
            </el-badge>
            <i class="el-icon-plus chat-icon" @click="addFriendDialogShow.show=true"></i>
            <el-dropdown>
              <i class="el-icon-setting chat-icon"></i>
              <el-dropdown-menu slot="dropdown">
                <el-dropdown-item @click.native="modifyPasswordDialogShow.show=true">修改密码</el-dropdown-item>
                <el-dropdown-item @click.native="loginout">退出登录</el-dropdown-item>
              </el-dropdown-menu>
            </el-dropdown>

            <el-drawer
              title="好友邀请"
              :visible.sync="drawer"
              :direction="direction"
              :show-close="false"
            >
              <invitation
                v-for="invit in invits"
                @addFriend="AddNewFriend"
                :invit="invit"
                :key="invit.invitId"
              />
            </el-drawer>
          </div>
        </div>
        <div class="custom-flex-column aside-right flex5" style="min-width: 0;">
          <div class="search-box">
            <div
              class="custom-flex-center search-box-content"
              v-show="!searchSeleted.isSelected"
              @click="selecteSearch"
            >
              <i class="el-icon-search search-icon"></i>
              <span class="search-text">搜索</span>
            </div>
            <div
              class="custom-flex-center search-box-content"
              style="background: #FFF;padding: 0 7px;"
              v-show="searchSeleted.isSelected"
            >
              <input
                style="outline:none;border:none;width: 100%"
                v-model="searchSeleted.content"
                @blur="leaveSearch"
                ref="searchInput"
              />
            </div>
          </div>
          <div class="friend-box">
            <div v-for="man in linkman" :key="man.userId">
              <linkman
                :class="{'linkman-selected':(currentLinkman.userId === man.userId)}"
                :contact="man"
                @selectLinkman="selectLinkman(man)"
              />
            </div>
          </div>
        </div>
      </el-aside>
      <el-container style="height: 500px">
        <el-header class="custom-flex-align-center" style="height: 50px;">
          <span>{{currentLinkman.userName}}</span>
        </el-header>
        <el-main style="overflow: auto" ref="main">
          <div v-for="chat in currentLinkman.chatList" :key="chat.id">
            <chat-message :chatMsg="chat" />
          </div>
        </el-main>
        <el-footer class="custom-flex-column" style="height: 150px;">
          <div class="flex1" v-if="currentLinkman.userId?true:false">
            <el-popover placement="top" width="400" v-model="visible" style="padding: 8px;">
              <span
                v-for="(e,index) in emoji"
                :key="index"
                class="emoji-sty"
                @click="addEmojiToChat(e)"
              >{{e}}</span>
              <i class="el-icon-orange" slot="reference"></i>
            </el-popover>
          </div>
          <div
            contenteditable="true"
            class="flex5"
            style="outline:none;overflow:auto"
            @blur="changeMsg"
            ref="edit"
          ></div>
          <div class="flex1">
            <el-row>
              <el-col :span="3" :offset="21">
                <el-button
                  style="padding:8px;"
                  @click="sendMsg"
                  v-if="currentLinkman.userId?true:false"
                >发送(s)</el-button>
              </el-col>
            </el-row>
          </div>
        </el-footer>
      </el-container>
    </el-container>

    <!-- 邀请好友dialog -->
    <addFriendDialog :addFriendDialogShow="addFriendDialogShow" />
    <!-- 修改密码dialog -->
    <changePasswordDialog :modifyPasswordDialogShow="modifyPasswordDialogShow" />
  </div>
</template>

<script>
import chatMessage from "@/components/chatMessage";
import linkman from "@/components/linkman";
import { emoji } from "@/common/const.js";
import { init } from "@/common/socket";
import UTILS from "@/common/util";
import invitation from "@/components/invitation";
import addFriendDialog from "@/components/addFriendDialog";
import changePasswordDialog from "@/components/changePasswordDialog";
import { CONSTANT } from "@/common/const";
export default {
  components: {
    chatMessage,
    linkman,
    invitation,
    addFriendDialog,
    changePasswordDialog
  },
  data() {
    return {
      emoji,
      visible: false,
      drawer: false,
      direction: "rtl",
      chatMsg: "",
      isExistNewInvitation: false,
      currentLinkman: {},
      linkman: [],
      invits: [],
      searchSeleted: {
        isSelected: false,
        content: ""
      },
      addFriendDialogShow: {
        show: false
      },
      modifyPasswordDialogShow: {
        show: false
      },
      methodDic: {
        AddInvitation: arg => {
          this.AddInvitation(arg);
        },
        AddNewFriend: user => {
          this.AddNewFriend(JSON.parse(user));
        },
        SendNoReadCount: userId => {
          this.SendNoReadCount(userId);
        },
        SendMsg: msg => {
          this.ReceiveMsg(JSON.parse(msg));
        }
      }
    };
  },
  created() {
    //初始化socket，这里应该在有效期内只执行一次
    //传入回调获取结果
    //这里要搞清楚调接口的顺序
    //哪些是不用同步的，哪些是需要同步的
    //不需要同步的，好友邀请的数量
    //获取邀请数量
    this.getIsExistNewInvitation();
    //需要同步的，获取好友列表，然后获取每个好友的未读信息，获取最后一条信息
    //获取好友列表
    this.getAllFriend();
  },
  methods: {
    receive(res) {
      if (res.target === "heartBeat") {
        return;
      }
      this.methodDic[res.target](...res.arguments);
    },
    AddInvitation(arg) {
      if (arg === "Success") {
        this.isExistNewInvitation = true;
      }
    },
    SendNoReadCount(userId) {
      if (this.currentLinkman.userId === userId) {
        return;
      }
      this.linkman.forEach(data => {
        if (data.userId === userId) {
          data.unReadCount++;
        }
      });
    },
    scrollToBottom() {
      this.$nextTick(() => {
        let container = this.$refs.main.$el;
        container.scrollTop = container.scrollHeight;
      });
    },
    //接收信息
    ReceiveMsg(msg) {
      this.linkman.forEach(data => {
        if (data.userId === msg.userId) {
          data.chatList.push({
            id: msg.messageId,
            msg: msg.message,
            isCurrentUser: false
          });
          data.latestNews = msg.message;
          this.scrollToBottom();
          //将该条信息置为已读
          this.$post(CONSTANT.API.READ_MESSAGE_BY_ID, {
            id: msg.messageId
          }).then(resp => {
            console.log(resp, "已读当前信息");
          });
        }
      });
    },
    AddNewFriend(user) {
      this.linkman.push({
        userId: user.Id,
        userName: user.UserName,
        latestNews: "你们已经是好友了，开始聊天吧",
        lastContactTime: UTILS.getNowDate(),
        unReadCount: 1,
        chatList: [
          {
            id: 1,
            msg: "你们已经是好友了，开始聊天吧",
            isCurrentUser: true
          }
        ]
      });
    },
    getAllFriend() {
      this.$post(CONSTANT.API.FIND_FRIEND).then(resp => {
        resp.data.forEach(data => {
          this.linkman.push({
            userId: data.userId,
            userName: data.nickname,
            latestNews: "你们已经是好友了，开始聊天吧",
            lastContactTime: UTILS.getNowDate(),
            unReadCount: 0,
            chatList: []
          });
        });
        console.log(resp, "获取好友");
        this.getNewMessageCount();
        init(this.receive);
      });
    },
    getNewMessageCount() {
      this.$post(CONSTANT.API.FIND_ALL_MESSAGE_COUNT).then(resp => {
        let map = new Map();
        resp.data.forEach(data => {
          map.set(data.userId, data.unRead);
        });
        this.linkman.forEach(data => {
          let unReadCount = map.get(data.userId);
          if (unReadCount) {
            data.unReadCount = unReadCount;
          }
        });
      });
    },
    //发送消息
    sendMsg() {
      this.$post(CONSTANT.API.SEND_MSG, {
        reciveUserId: this.currentLinkman.userId,
        message: this.chatMsg
      }).then(resp => {
        console.log(resp);
        this.currentLinkman.chatList.push({
          id: resp.data.id,
          msg: this.chatMsg,
          isCurrentUser: true
        });
        this.currentLinkman.latestNews = this.chatMsg;
        //清空输入内容
        this.clearEdit();
        this.scrollToBottom();
      });
    },
    //清空信息框
    clearEdit() {
      let edit = this.$refs.edit;
      edit.innerText = "";
    },
    //改变聊天信息的内容
    changeMsg(e) {
      this.chatMsg = e.target.innerText;
    },
    //添加表情到聊天信息
    addEmojiToChat(emo) {
      let edit = this.$refs.edit;
      edit.innerText += emo;
      this.chatMsg = edit.innerText;
      this.visible = false;
    },
    //选择联系人
    selectLinkman(man) {
      this.currentLinkman = man;
      //获取所有未读信息,并设置已读
      this.$post(CONSTANT.API.FIND_ALL_NOREAD_MESSAGE, {
        currentChatUserId: man.userId
      }).then(resp => {
        resp.data.forEach(data => {
          man.chatList.push({
            id: data.userId,
            msg: data.message,
            isCurrentUser: false
          });
        });
      });
      man.unReadCount = 0;
      this.clearEdit();
    },
    leaveSearch() {
      if (this.searchSeleted.content !== "") {
        return;
      }
      this.searchSeleted.isSelected = false;
    },
    selecteSearch() {
      this.searchSeleted.isSelected = true;
      this.$nextTick(() => {
        this.$refs.searchInput.focus();
      });
    },
    //注销
    loginout() {
      this.$message
        .confirm("是否确定退出登录？", "提示", {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning",
          center: true
        })
        .then(() => {
          this.$get(CONSTANT.API.LOGIN_OUT).then(() => {
            localStorage.removeItem("user");
            this.$message({
              type: "success",
              message: "退出成功!"
            });
            this.$router.push("/login");
          });
        })
        .catch(() => {});
    },
    getIsExistNewInvitation() {
      this.$post(CONSTANT.API.INVITATION_COUNT).then(resp => {
        if (resp.data > 0) {
          this.isExistNewInvitation = true;
        }
      });
    },
    //查找邀请并将所有邀请置为已读
    findInvitation() {
      this.$post(CONSTANT.API.FIND_ALL_INVITATION).then(resp => {
        //{ invitId: 3, username: "zws", state: 2 }
        this.invits = [];
        resp.data.forEach(data => {
          this.invits.push({
            invitId: data.id,
            username: data.userName,
            state: data.isAccept,
            chumId: data.userId
          });
        });
        console.log(resp);
        this.drawer = true;
        this.readedAllInvitation();
      });
    },
    readedAllInvitation() {
      this.$post(CONSTANT.API.READ_INVITATION).then(resp => {
        console.log(resp, "已读所有邀请信息");
        this.isExistNewInvitation = false;
      });
    }
  }
};
</script>

<style scoped>
.el-header,
.el-footer {
  color: #333;
}
.el-footer {
  background-color: #f5f5f5;
  border-top: 1px solid rgba(0, 0, 0, 0.09);
  padding: 10px;
}
.el-header {
  background-color: #f5f5f5;
  border-bottom: 1px solid rgba(0, 0, 0, 0.09);
}
.el-aside {
  background-color: rgba(0, 0, 0, 0.1);
  color: #333;
  text-align: center;
  border-right: 1px solid rgba(0, 0, 0, 0.09);
}

.el-main {
  background-color: #f5f5f5;
  color: #333;
}
/* 隐藏滚动条 */
.el-main::-webkit-scrollbar {
  width: 0 !important;
}
body > .el-container {
  margin-bottom: 40px;
}
.chat-box {
  width: 900px;
}
.chat-icon {
  color: #dcdcdc81;
  font-size: 20px;
  margin-top: 20px;
}
.header {
  width: 40px;
  height: 40px;
}
.margin-top30 {
  margin-top: 30px;
}
.container-border {
  border: 1px solid rgba(0, 0, 0, 0.1);
}
.aside-left {
  height: 100%;
  background: rgba(0, 0, 0, 0.8);
  padding: 0 5px;
}
.aside-right {
  height: 100%;
}
.flex1 {
  flex: 1;
}
.flex5 {
  flex: 5;
}
.friend-box {
  background: #f5f5f5;
  height: 440px;
  width: 100%;
  overflow: auto;
}
.friend-box::-webkit-scrollbar {
  width: 0 !important;
}
.search-box {
  height: 40px;
  background: #dcdcdc;
  padding: 10px;
}
.search-box-content {
  background: rgba(0, 0, 0, 0.08);
  border-radius: 5px;
  height: 30px;
  margin-top: 3px;
}
.search-icon {
  color: rgba(0, 0, 0, 0.5);
  font-size: 13px;
}
.search-text {
  color: rgba(0, 0, 0, 0.5);
  font-size: 10px;
}
.emoji-sty {
  cursor: pointer;
  display: inline-block;
  padding: 2px;
  font-size: 16px;
}
.linkman-selected {
  background: rgba(0, 0, 0, 0.09);
}
/deep/ .el-badge__content.is-fixed.is-dot {
  right: 18px;
  top: 22px;
}
</style>