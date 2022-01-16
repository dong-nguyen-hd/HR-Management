<template>
  <div class="row" style="height: 90vh">
    <div
      v-bind:class="{
        'justify-center': $q.screen.md || $q.screen.sm || $q.screen.xs,
      }"
      class="col-12 col-md-6 flex content-center"
    >
      <q-card
        v-bind:style="$q.screen.lt.sm ? { width: '80%' } : { width: '50%' }"
      >
        <q-card-section>
          <q-avatar size="103px" class="absolute-center shadow-10">
            <img src="~assets/login.jpg" alt="avatar" />
          </q-avatar>
        </q-card-section>
        <q-card-section>
          <div class="q-pt-lg">
            <div class="col text-h6 ellipsis flex justify-center">
              <h2 class="text-h2 text-uppercase q-my-none text-weight-regular">
                Login
              </h2>
            </div>
          </div>
        </q-card-section>
        <q-card-section>
          <q-form class="q-gutter-md" @submit.prevent="submitForm">
            <q-input label="Username" v-model="account.username"> </q-input>
            <q-input label="Password" type="password" v-model="account.password">
            </q-input>
            <div>
              <q-btn
                class="full-width"
                color="primary"
                label="Login"
                type="submit"
                rounded
              ></q-btn>
              <div class="text-center q-mt-sm q-gutter-lg">
                <router-link class="text-white" to="/login"
                  >Forgot password?</router-link
                >
                <router-link class="text-white" to="/login"
                  >Create an account</router-link
                >
              </div>
            </div>
          </q-form>
        </q-card-section>
      </q-card>
    </div>
  </div>
</template>

<script>
import { useQuasar } from "quasar";
import { mapActions } from "vuex";

let $q;

export default {
  name: "Login",
  data() {
    return {
      account: {
        username: "admin",
        password: "d7f32454b44d22182618d56e683f419a",
      },
    };
  },
  methods: {
    ...mapActions("auth", ["login"]),
    async submitForm() {
      if (!this.account.username || !this.account.password) {
        $q.notify({
          type: "negative",
          message: "Informed data are invalid.",
        });
      } else if (this.account.password.length < 6) {
        $q.notify({
          type: "negative",
          message: "The password must have 6 or more characters.",
        });
      } else {
        try {
          await this.login(this.account);
          const toPath = this.$route.query.to || "/admin";
          this.$router.push(toPath);
        } catch (err) {
          if (err.response.data.detail) {
            $q.notify({
              type: "negative",
              message: err.response.data.detail,
            });
          }
        }
      }
    },
  },
  mounted() {
    $q = useQuasar();
  },
};
</script>

<style lang="scss" scoped>

</style>
