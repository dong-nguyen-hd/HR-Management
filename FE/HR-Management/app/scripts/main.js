import Vue from 'vue';
import App from '@/views/layouts/app';
import BalmUI from 'balm-ui'; // Official Google Material Components
import BalmUIPlus from 'balm-ui/dist/balm-ui-plus'; // BalmJS Team Material Components

Vue.config.productionTip = false;
Vue.use(BalmUI); // Mandatory
Vue.use(BalmUIPlus); // Optional

new Vue({
  el: '#app',
  components: { App },
  template: '<app/>'
});

