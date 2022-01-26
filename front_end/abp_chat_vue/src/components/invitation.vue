<template>
  <el-row style="border-bottom: 1px solid rgba(0,0,0,.1);padding: 5px 0;cursor: pointer;">
    <el-col :span="10" :offset="2" class="custom-flex-align-center">
      <img src="../assets/static/header.jpg" style="width: 40px;height: 40px;border-radius:50%" />
      <span style="font-size: 20px;margin-left: 10px;">{{invit.username}}</span>
    </el-col>
    <el-col :span="12" style="padding-top: 7px">
      <div v-if="invit.state === 2">
        <el-button style="padding:8px;" type="success" @click="aggreeAddFriend">同意</el-button>
        <el-button style="padding:8px;" type="warning" @click="refuseAddFriend">拒绝</el-button>
      </div>
      <span style="font-size: 15px;color: rgba(0,0,0,.3);" v-if="invit.state === 0">已拒绝</span>
      <span style="font-size: 15px;color: rgba(0,0,0,.3);" v-if="invit.state === 1">已接受</span>
    </el-col>
  </el-row>
</template>

<script>
import { CONSTANT } from "@/common/const";
export default {
  props: {
    invit: {
      invitId: "",
      username: "",
      chumId: "",
      state: 0 //0新邀请，1同意邀请，2拒绝邀请
    }
  },
  data() {
    return {};
  },
  methods: {
    aggreeAddFriend() {
      this.$post(CONSTANT.API.ADD_FRIEND, {
        friendId: this.invit.chumId,
        invitationId: this.invit.invitId
      }).then(resp => {
        console.log(resp, "成功添加好友");
        this.invit.state = 1;
        //添加好友
        let user = { Id: resp.data.userId, UserName: resp.data.nickname };
        this.$emit("addFriend", user);
      });
    },
    refuseAddFriend() {
      this.$post(CONSTANT.API.REFUSE_INVITATION, {
        invitationId: this.invit.invitId
      }).then(resp => {
        console.log(resp, "拒绝添加好友");
        this.invit.state = 0;
      });
    }
  }
};
</script>

<style scoped>
</style>