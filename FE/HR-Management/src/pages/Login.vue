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
          <div class="top-left-login display-center">
            <div class="login-pic">
              <img
                src="../assets/images/logo-vertical.svg"
                alt="Human Resource Management"
              />
            </div>
          </div>

          <div class="top-right-login display-center">
            <q-form
              @submit.prevent="submitForm"
              class="q-pa-none width-form-login"
            >
              <div class="title-login display-center">
                <div class="login-pic lt-md q-pr-sm">
                  <img
                    src="../assets/images/logo-mono-black.svg"
                    alt="Human Resource Management"
                  />
                </div>
                Welcome!
              </div>
              <q-input
                class="q-pb-sm"
                tabindex="1"
                rounded
                hide-bottom-space
                outlined
                v-model="account.userName"
                placeholder="User name"
                lazy-rules="ondemand"
                :rules="[
                  (val) =>
                    (val && val.length > 0) || 'User name can not be empty!',
                ]"
              >
                <template v-slot:prepend>
                  <q-icon size="20px" name="fas fa-user" />
                </template>
              </q-input>

              <q-input
                class="q-pb-lg"
                hide-bottom-space
                tabindex="2"
                rounded
                outlined
                type="password"
                v-model="account.password"
                placeholder="Password"
                lazy-rules="ondemand"
                :rules="[
                  (val) =>
                    (val && val.length > 0) || 'Password can not be empty!',
                  (val) =>
                    (val && val.length > 5) ||
                    'Password must be more than 5 characters',
                ]"
              >
                <template v-slot:prepend>
                  <q-icon size="20px" name="fas fa-lock" />
                </template>
              </q-input>

              <q-btn
                unelevated
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
import { mapActions, mapGetters } from "vuex";
import MD5 from "crypto-js/md5";
import { useQuasar } from "quasar";
import { api } from "src/boot/axios";

export default {
  name: "Login",
  data() {
    return {
      account: {
        // You need to remove account information!
        userName: "admin",
        password: "admin1234",
      },
    };
  },
  methods: {
    ...mapActions("auth", ["login"]),

    async submitForm() {
      try {
        // Hashing password
        let payload = {
          userName: this.account.userName,
          password: MD5(this.account.password).toString(),
        };

        // Request API
        let result = await api
          .post("/api/v1/token/login", payload)
          .then((response) => {
            return response.data;
          })
          .catch(function (error) {
            // Checking if throw error
            if (error.response) {
              // Server response
              return error.response.data;
            } else {
              // Server not working
              let temp = { success: false, message: ["Server Error!"] };
              return temp;
            }
          });

        if (result.success) {
          await this.login(result);
          this.$router.replace("/list-employee");
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } catch (err) {
        this.$q.notify({
          type: "negative",
          message: "Server Error!",
        });
      }
    },
  },
  computed: {
    ...mapGetters("auth", ["isAuthenticated"]),
  },
  created() {
    if (this.isAuthenticated) {
      this.$router.replace("/list-employee");
    }
  },
  mounted() {
    const $q = useQuasar();
  },
  watch: {
    isAuthenticated: function () {
      if (this.isAuthenticated) {
        this.$router.replace("/list-employee");
      }
    },
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

.btn-login {
  width: 100%;
  height: 56px;
  font-family: Montserrat-Bold;
  font-size: 15px;
  line-height: 1.5;

  .q-btn:hover {
    background-color: $accent;
  }
}

.display-center {
  display: flex;
  justify-content: center;
  align-items: center;
}

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
    width: 60%;
  }

  .top-left-login,
  .top-right-login {
    width: calc(100% - 50%);
  }

  .width-form-login {
    width: 70%;
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
    width: 16%;
  }

  .width-form-login {
    width: 80%;
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
    width: 16%;
  }

  .width-form-login {
    width: 80%;
  }
}
</style>
