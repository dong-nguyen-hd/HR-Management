<!--
This template is inspired by colorlib
Author: Colorlib
License: CC BY 3.0
-->

<template>
  <div class="limiter">
    <div class="container-login display-center">
      <div class="wrap-login">
        <div class="top-login">
          <div class="login-pic top-left-login display-center">
            <img
              src="../assets/images/img-01.png"
              alt="Human Resource Management"
            />
          </div>

          <div class="top-right-login display-center">
            <q-form
              @submit.prevent="submitForm"
              class="q-pa-none width-form-login"
            >
              <div class="title-login display-center">
                <div class="login-pic lt-md q-pr-sm">
                  <img
                    src="../assets/images/img-01.png"
                    alt="Human Resource Management"
                  />
                </div>
                Login
              </div>
              <q-input
                class="q-pb-sm"
                tabindex="1"
                rounded
                hide-bottom-space
                outlined
                v-model="account.username"
                placeholder="User name"
                error-message="test"
                lazy-rules="ondemand"
                :rules="[
                  (val) => (val && val.length > 0) || 'Please type something',
                ]"
              >
                <template v-slot:prepend>
                  <q-icon size="20px" name="fas fa-user" />
                </template>
              </q-input>

              <q-input
                class="q-pb-lg"
                tabindex="2"
                rounded
                outlined
                type="password"
                v-model="account.password"
                placeholder="Password"
                lazy-rules="ondemand"
                :rules="[
                  (val) => (val && val.length > 0) || 'Please type something',
                ]"
              >
                <template v-slot:prepend>
                  <q-icon size="20px" name="fas fa-lock" />
                </template>
              </q-input>

              <q-btn
                class="btn-login"
                tabindex="3"
                rounded
                label="Login"
                type="submit"
                color="primary"
              />
            </q-form>
          </div>
        </div>

        <div class="bottom-login display-center">
          <a href="https://github.com/dong-nguyen-hd" target="_blank">
            <q-icon size="18px" name="fab fa-github" />
            Dong Nguyen
          </a>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { useQuasar } from "quasar";
import { mapActions } from "vuex";
import MD5 from "crypto-js/md5";

let $q;

export default {
  name: "Login",
  data() {
    return {
      account: {
        username: "",
        password: "",
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
      } else if (this.account.password.length < 3) {
        $q.notify({
          type: "negative",
          message: "The password must have 6 or more characters.",
        });
      } else {
        try {
          // Hashing password
          this.account.password = MD5(this.account.password).toString();

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
.limiter {
  width: 100%;
  height: 100vh;
  .container-login {
    width: 100%;
    height: 100%;
    background: #9053c7;
    background: -webkit-linear-gradient(-135deg, #c850c0, #4158d0);
    background: -o-linear-gradient(-135deg, #c850c0, #4158d0);
    background: -moz-linear-gradient(-135deg, #c850c0, #4158d0);
    background: linear-gradient(-135deg, #c850c0, #4158d0);
    .wrap-login {
      background: #fff;
      border-radius: 10px;
      overflow: hidden;
      display: flex;
      flex-direction: column;
      flex-wrap: nowrap;
      .top-login {
        display: flex;
        flex-direction: row;
        width: 100%;
        height: calc(100% - 10%);
      }
      .bottom-login {
        height: calc(100% - 90%);
        width: 100%;
      }
    }
  }
}

.bottom-login {
  a {
    font-size: 14px;
    line-height: 1.7;
    color: $grey;
    transition: all 0.4s;
    -webkit-transition: all 0.4s;
    -o-transition: all 0.4s;
    -moz-transition: all 0.4s;
    text-decoration: none;
  }

  a:focus {
    outline: none !important;
  }

  a:hover {
    text-decoration: none;
    color: $accent;
  }
}

.title-login {
  font-family: Poppins-Bold;
  font-size: 24px;
  color: $grey;
  line-height: 1.2;
  padding-bottom: 20px;
}

.width-form-login {
  width: 80%;
}

.btn-login {
  width: 100%;
  height: 56px;
}

.display-center {
  display: flex;
  justify-content: center;
  align-items: center;
}

/*------------------------------------------------------------------
[ Resize picture ]*/

.login-pic {
  img {
    max-width: 100%;
  }
}

/*------------------------------------------------------------------
[ Responsive ]*/

@media (min-width: 1024px) {
  .wrap-login {
    width: calc(100% - 38%);
    height: calc(100% - 8%);
  }

  .login-pic {
    width: 34%;
  }

  .top-left-login,
  .top-right-login {
    width: calc(100% - 50%);
  }
}

@media (max-width: 1023px) {
  .wrap-login {
    width: calc(100% - 38%);
    height: calc(100% - 8%);
  }

  .top-left-login {
    display: none;
  }

  .top-right-login {
    width: 100%;
  }

  .login-pic {
    width: 20%;
  }
}

@media (max-width: 599px) {
  .wrap-login {
    width: calc(100% - 8%);
    height: calc(100% - 24%);
  }

  .top-left-login {
    display: none;
  }

  .top-right-login {
    width: 100%;
  }

  .login-pic {
    width: 20%;
  }
}
</style>
