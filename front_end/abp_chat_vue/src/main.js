import Vue from './ElementImport/import'
import './common/common.css'
import App from './App.vue'
import VueRouter from 'vue-router'
import routes from './router'
import { post, get } from './common/request';
import UTILS from './common/util';
import { MessageBox } from 'element-ui';
Vue.use(VueRouter)

Vue.config.productionTip = false
const router = new VueRouter({
  routes
});
router.beforeEach((to, from, next) => {
  if (to.name !== 'Login' && !UTILS.isLogined()) {
    next({ name: 'Login' });
  }
  else {
    next();
  }
})
Vue.prototype.$message = MessageBox;
Vue.prototype.$post = post;
Vue.prototype.$get = get;
new Vue({
  router,
  render: h => h(App),
}).$mount('#app')
