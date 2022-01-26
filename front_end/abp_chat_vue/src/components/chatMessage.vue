<template>
  <div>
    <!-- 左边聊天信息 -->
    <div class="custom-flex-between margin-bottom20" v-if="!chatMsg.isCurrentUser">
      <div class="custom-flex-align-center">
        <img alt="header" class="img-sty" src="../assets/static/header.jpg" />
        <div class="left-arrow"></div>
        <div class="chat-box chat-left-back">
          <span class="font-size15">{{chatMsg.msg}}</span>
        </div>
      </div>
      <div></div>
    </div>
    <!-- 右边聊天信息 -->
    <div class="custom-flex-between margin-bottom20" v-if="chatMsg.isCurrentUser">
      <div></div>
      <div class="custom-flex-align-center">
        <div class="chat-box chat-right-back">
          <span class="font-size15">{{chatMsg.msg}}</span>
        </div>
        <div class="right-arrow"></div>
        <img alt="header" class="img-sty" src="../assets/static/header.jpg" />
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    chatMsg: {
      id: "",
      msg: "",
      isCurrentUser: true //是否是当前用户，true聊天信息放右边，false放左边
    }
  }
};
</script>

<style scoped>
.margin-bottom20 {
  margin-bottom: 20px;
}
.img-sty {
  width: 40px;
  height: 40px;
  border-radius: 50%;
}
.chat-box {
  padding: 5px;
  max-width: 300px;
  border-radius: 7px;
}
.chat-left-back {
  background: #fff;
}
.chat-right-back {
  background: rgb(0, 255, 0);
}
.left-arrow {
  border-top: 8px solid #f5f5f5;
  border-bottom: 8px solid #f5f5f5;
  border-right: 8px solid #fff;
  margin-left: 10px;
}
.right-arrow {
  border-top: 8px solid #f5f5f5;
  border-bottom: 8px solid #f5f5f5;
  border-left: 8px solid rgb(0, 255, 0);
  margin-right: 10px;
}
.font-size15 {
  font-size: 15px;
}
</style>