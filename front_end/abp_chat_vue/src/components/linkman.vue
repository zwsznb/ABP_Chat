<template>
  <div class="linkman-box" @click="selectLinkman">
    <el-row class="custom-flex-align-center">
      <el-badge
        :value="contact.unReadCount"
        :hidden="contact.unReadCount===0"
        :max="99"
        class="item"
      >
        <el-col :span="4" :offset="2">
          <img alt="header" class="header" src="../assets/static/header.jpg" />
        </el-col>
      </el-badge>
      <el-col :span="10" :offset="1" class="custom-flex-column" style="text-align:left">
        <span style="font-size: 18px;">{{contact.userName}}</span>
        <span class="linkman-last-msg">{{contact.latestNews}}</span>
      </el-col>
      <el-col :span="8" style="height: 35px">
        <span class="date">{{contact.lastContactTime}}</span>
      </el-col>
    </el-row>
  </div>
</template>

<script>
export default {
  props: {
    contact: {
      userId: "",
      userName: "",
      latestNews: "",
      lastContactTime: "",
      unReadCount: 0
    }
  },
  methods: {
    selectLinkman() {
      this.$emit("selectLinkman", this.contact);
    }
  }
};
</script>

<style scoped>
.linkman-box {
  height: 40px;
  width: 100%;
  padding: 11px 0;
  cursor: pointer;
}
.linkman-last-msg {
  color: rgba(0, 0, 0, 0.3);
  font-size: 10px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
.date {
  color: rgba(0, 0, 0, 0.3);
  font-size: 10px;
}
.header {
  width: 40px;
  height: 40px;
}
.item {
  font-size: 5px;
}
</style>