<template>
  <q-page class="full-height full-width flex flex-center">
    <div class="container full-height full-width">
      <q-dialog v-model="deletePay" :persistent="payProcess">
        <q-card>
          <q-card-section class="row items-center">
            <span class="text-h6">{{$t('deleteSalary')}}</span>
          </q-card-section>

          <q-separator />

          <q-card-section>
            <span
              >{{$t('confirmDelete')}}</span
            >
          </q-card-section>

          <q-card-actions align="right">
            <q-btn
              flat
              :disable="payProcess"
              :label="$t('cancel')"
              color="primary"
              v-close-popup
            />
            <q-btn
              flat
              :label="$t('btnDelete')"
              color="negative"
              @click="deletePayAsync"
              :loading="payProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <q-dialog v-model="insertPay" :persistent="payProcess">
        <q-card style="width: 400px">
          <q-card-section class="row items-center">
            <span class="text-h6">{{ $t('createSalary') }}</span>
          </q-card-section>

          <q-separator />

          <q-card-section>
            <div class="q-mt-sm">
              <q-input
                standout
                v-model="insertPayObj.firstName"
                type="text"
                :label="$t('firstNameColon')"
                readonly
              >
              </q-input>
            </div>

            <div class="q-mt-sm">
              <q-input
                readonly
                standout
                v-model="insertPayResource.date"
                type="text"
                placeholder="YYYY-MM-DD"
                stack-label
                :label="$t('date')"
                label-color="white"
                bg-color="primary"
                input-class="text-white"
              >
                <template v-slot:append>
                  <q-icon name="event" class="cursor-pointer" color="white">
                    <q-popup-proxy
                      cover
                      transition-show="scale"
                      transition-hide="scale"
                    >
                      <q-date
                        v-model="insertPayResource.date"
                        mask="YYYY-MM-DD"
                      >
                        <div class="row items-center justify-end">
                          <q-btn
                            v-close-popup
                            label="Close"
                            color="primary"
                            flat
                          />
                        </div>
                      </q-date>
                    </q-popup-proxy>
                  </q-icon>
                </template>
              </q-input>
            </div>

            <div class="q-mt-sm">
              <q-input
                ref="oneRef"
                standout
                maxlength="250"
                v-model="insertPayResource.baseSalary"
                type="number"
                :label="$t('baseSalaryColon')"
                :label-color="colorFocusPay[0]"
                @focus="colorFocusPay[0] = 'white'"
                @blur="colorFocusPay[0] = ''"
                :rules="[(val) => !!val || 'Base salary is required']"
                lazy-rules="ondemand"
                hide-bottom-space
              >
              </q-input>
            </div>

            <div class="q-mt-sm">
              <q-input
                ref="twoRef"
                standout
                maxlength="250"
                v-model="insertPayResource.allowance"
                type="number"
                :label="$t('allowanceColon')"
                :label-color="colorFocusPay[1]"
                @focus="colorFocusPay[1] = 'white'"
                @blur="colorFocusPay[1] = ''"
              >
              </q-input>
            </div>

            <div class="q-mt-sm">
              <q-input
                ref="threeRef"
                standout
                maxlength="250"
                v-model="insertPayResource.bonus"
                type="number"
                :label="$t('bonusColon')"
                :label-color="colorFocusPay[2]"
                @focus="colorFocusPay[2] = 'white'"
                @blur="colorFocusPay[2] = ''"
              >
              </q-input>
            </div>
          </q-card-section>

          <q-card-actions align="right">
            <q-btn
              flat
              :disable="payProcess"
              :label="$t('cancel')"
              color="primary"
              v-close-popup
            />
            <q-btn
              flat
              :label="$t('btnAdd')"
              color="info"
              @click="createPay"
              :loading="payProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <div
        class="table-component full-height full-width flex flex-center q-px-md"
      >
        <div class="new-item q-mb-md flex justify-end full-width">
          <q-input
            dense
            readonly
            clearable
            standout
            v-model="filter.date"
            type="text"
            placeholder="YYYY-MM-DD"
            stack-label
            :label="$t('date')"
            label-color="white"
            bg-color="primary"
            input-class="text-white"
            hide-bottom-space
          >
            <template v-slot:append>
              <q-icon name="event" class="cursor-pointer" color="white">
                <q-popup-proxy
                  cover
                  transition-show="scale"
                  transition-hide="scale"
                >
                  <q-date
                    v-model="filter.date"
                    mask="YYYY-MM-DD"
                    @update:model-value="getEmployeeWithFilter(false)"
                  >
                    <div class="row items-center justify-end">
                      <q-btn v-close-popup label="Close" color="primary" flat />
                    </div>
                  </q-date>
                </q-popup-proxy>
              </q-icon>
            </template>
          </q-input>
        </div>

        <q-table
          class="table-content"
          :rows="listEmployee"
          :columns="headerTable"
          row-key="id"
          flat
          bordered
          dark
          :loading="loadingData"
          v-model:pagination="pagination"
          :rows-per-page-options="[5, 10, 15, 20]"
          @request="getEmployeeWithFilter"
        >
          <template v-slot:header-cell-staffId="props">
            <q-th :props="props">
              <div :style="`min-width: ${widthOfStaffId}`">
                <q-input
                  @update:model-value="getEmployeeWithFilter(false)"
                  dark
                  dense
                  standout
                  v-model="filter.staffId"
                  input-class="text-right"
                  :label="labelNameFocus[0]"
                  :label-color="labelColorFocus[0]"
                  debounce="300"
                  @focus="
                    labelColorFocus[0] = 'black';
                    labelNameFocus[0] = $t('searchByStaffId');
                    widthOfStaffId = '154px';
                  "
                  @blur="
                    labelColorFocus[0] = 'white';
                    labelNameFocus[0] = props.col.label;
                    widthOfStaffId = '80px';
                  "
                >
                  <template v-slot:append>
                    <q-icon v-if="!filter.staffId" name="search" />
                    <q-icon
                      v-else
                      name="clear"
                      class="cursor-pointer"
                      @click="
                        filter.staffId = '';
                        getEmployeeWithFilter(false);
                      "
                    />
                  </template>
                </q-input>
              </div>
            </q-th>
          </template>

          <template v-slot:header-cell-fullName="props">
            <q-th :props="props">
              <div :style="`min-width: ${widthOfFullName};`">
                <q-input
                  @update:model-value="getEmployeeWithFilter(false)"
                  dark
                  dense
                  standout
                  debounce="400"
                  v-model="filter.firstName"
                  input-class="text-right"
                  :label="labelNameFocus[1]"
                  :label-color="labelColorFocus[1]"
                  @focus="
                    labelColorFocus[1] = 'black';
                    labelNameFocus[1] = $t('searchByFirstName');
                    widthOfFullName = '170px';
                  "
                  @blur="
                    labelColorFocus[1] = 'white';
                    labelNameFocus[1] = props.col.label;
                    widthOfFullName = '100px';
                  "
                >
                  <template v-slot:append>
                    <q-icon v-if="!filter.firstName" name="search" />
                    <q-icon
                      v-else
                      name="clear"
                      class="cursor-pointer"
                      @click="
                        filter.firstName = '';
                        getEmployeeWithFilter(false);
                      "
                    />
                  </template>
                </q-input>
              </div>
            </q-th>
          </template>

          <template v-slot:header-cell-department="props">
            <q-th :props="props">
              <div :style="`min-width: ${widthOfDepartment};`">
                <q-select
                  @update:model-value="getEmployeeWithFilter(false)"
                  dense
                  standout
                  dark
                  clearable
                  v-model="filter.departmentId"
                  :options="tempListDepartment"
                  :label="props.col.label"
                  option-value="id"
                  option-label="name"
                  emit-value
                  map-options
                  options-selected-class="text-accent"
                  @filter="filterDepartment"
                  input-debounce="100"
                  fill-input
                  hide-selected
                  use-input
                  :label-color="labelColorFocus[2]"
                  @focus="
                    labelColorFocus[2] = 'black';
                    widthOfDepartment = '120px';
                  "
                  @blur="
                    labelColorFocus[2] = 'white';
                    widthOfDepartment = !filter.departmentId
                      ? '100px'
                      : '120px';
                  "
                  ><template v-slot:no-option>
                    <q-item>
                      <q-item-section class="text-grey">
                        {{ $t('noResults') }}
                      </q-item-section>
                    </q-item>
                  </template>
                </q-select>
              </div>
            </q-th>
          </template>

          <template v-slot:header-cell-position="props">
            <q-th :props="props">
              <div :style="`min-width: ${widthOfPosition};`">
                <q-select
                  @update:model-value="getEmployeeWithFilter(false)"
                  dense
                  standout
                  dark
                  clearable
                  v-model="filter.positionId"
                  :options="tempListPosition"
                  :label="props.col.label"
                  option-value="id"
                  option-label="name"
                  emit-value
                  map-options
                  options-selected-class="text-accent"
                  @filter="filterPosition"
                  input-debounce="100"
                  fill-input
                  hide-selected
                  use-input
                  :label-color="labelColorFocus[3]"
                  @focus="
                    labelColorFocus[3] = 'black';
                    widthOfPosition = '120px';
                  "
                  @blur="
                    labelColorFocus[3] = 'white';
                    widthOfPosition = !filter.positionId ? '100px' : '120px';
                  "
                  ><template v-slot:no-option>
                    <q-item>
                      <q-item-section class="text-grey">
                        No results
                      </q-item-section>
                    </q-item>
                  </template>
                </q-select>
              </div>
            </q-th>
          </template>

          <template v-slot:body-cell-avatar="props">
            <q-td :props="props">
              <q-avatar>
                <img :src="props.value" />
              </q-avatar>
            </q-td>
          </template>

          <template v-slot:body-cell-action="props">
            <q-td :props="props">
              <div>
                <q-btn
                  style="width: 60px"
                  dense
                  color="white"
                  text-color="black"
                  :label="$t('new')"
                  v-show="!showOrHide(props.value)"
                  @click="openInsertPay(props.value)"
                />
              </div>
              <div class="q-mt-sm">
                <q-btn
                  style="width: 60px"
                  dense
                  color="negative"
                  :label="$t('delete')"
                  v-show="showOrHide(props.value)"
                  @click="openDeletePay(props.value)"
                />
              </div>
            </q-td>
          </template>

          <template v-slot:body-cell-fullName="props">
            <q-td :props="props">
              {{ showName(props.value) }}
            </q-td>
          </template>

          <template v-slot:body-cell-allowance="props">
            <q-td :props="props">
              {{ props.value ? numberWithSpaces(props.value.allowance) : "" }}
            </q-td>
          </template>

          <template v-slot:body-cell-bonus="props">
            <q-td :props="props">
              {{ props.value ? numberWithSpaces(props.value.bonus) : "" }}
            </q-td>
          </template>

          <template v-slot:body-cell-baseSalary="props">
            <q-td :props="props">
              {{ props.value ? numberWithSpaces(props.value.baseSalary) : "" }}
            </q-td>
          </template>

          <template v-slot:body-cell-net="props">
            <q-td :props="props">
              {{ props.value ? numberWithSpaces(props.value.net) : "" }}
            </q-td>
          </template>

          <template v-slot:body-cell-gross="props">
            <q-td :props="props">
              {{ props.value ? numberWithSpaces(props.value.gross) : "" }}
            </q-td>
          </template>

          <template v-slot:body-cell-healthInsurance="props">
            <q-td :props="props">
              {{
                props.value ? numberWithSpaces(props.value.healthInsurance) : ""
              }}
            </q-td>
          </template>

          <template v-slot:body-cell-socialInsurance="props">
            <q-td :props="props">
              {{
                props.value ? numberWithSpaces(props.value.socialInsurance) : ""
              }}
            </q-td>
          </template>

          <template v-slot:body-cell-pit="props">
            <q-td :props="props">
              {{ props.value ? numberWithSpaces(props.value.pit) : "" }}
            </q-td>
          </template>

          <template v-slot:loading>
            <q-inner-loading showing color="primary" />
          </template>
        </q-table>
      </div>
    </div>
  </q-page>
</template>

<script>
import { defineComponent } from "vue";
import { mapActions, mapGetters } from "vuex";
import { useQuasar, date } from "quasar";
import { api } from "src/boot/axios";

export default defineComponent({
  name: "List Employee For KT",

  data() {
    return {
      labelColorFocus: ["white", "white", "white", "white"],
      labelNameFocus: [this.$t('staffID'), this.$t('fullName'), this.$t('department'), this.$t('position')],

      colorFocusPay: [],

      widthOfStaffId: "80px",
      widthOfFullName: "130px",
      widthOfDepartment: "100px",
      widthOfPosition: "100px",

      loadingData: false,

      insertPay: false,
      deletePay: false,
      idDelete: 0,
      payProcess: false,
      insertPayObj: null,

      insertPayResource: {
        baseSalary: 0,
        allowance: 0,
        bonus: 0,
        date: "",
        personId: 0,
      },

      filter: {
        staffId: null,
        firstName: null,
        positionId: null,
        departmentId: null,
        date: null,
      },

      pagination: {
        page: 1,
        rowsPerPage: 10,
        rowsNumber: 10, // is total records

        firstPage: 1,
        lastPage: null,
        nextPage: null,
        previousPage: null,
        totalPages: 0,
      },

      tempListDepartment: [],
      listDepartment: [],
      tempListPosition: [],
      listPosition: [],
      listEmployee: [],
      headerTable: [
        {
          name: "index",
          label: "#",
          align: "center",
          field: "index",
        },
        {
          name: "staffId",
          align: "left",
          label: this.$t('staffID'),
          field: "staffId",
        },
        {
          name: "fullName",
          align: "left",
          label: this.$t('fullName'),
          field: (row) => `${row.firstName} ${row.lastName}`,
        },
        {
          name: "avatar",
          align: "center",
          label: "",
          field: (row) => row.avatar.thumbnail,
        },
        {
          name: "department",
          align: "left",
          label: this.$t('department'),
          field: (row) => row.department.name,
        },
        {
          name: "position",
          align: "left",
          label: this.$t('position'),
          field: (row) => row.position.name,
        },
        {
          name: "net",
          align: "center",
          label: "NET",
          field: (row) => row.pay,
        },
        {
          name: "gross",
          align: "center",
          label: "Gross",
          field: (row) => row.pay,
        },
        {
          name: "baseSalary",
          align: "center",
          label: this.$t('baseSalary'),
          field: (row) => row.pay,
        },
        {
          name: "allowance",
          align: "center",
          label: this.$t('allowance'),
          field: (row) => row.pay,
        },
        {
          name: "bonus",
          align: "center",
          label: this.$t('bonus'),
          field: (row) => row.pay,
        },
        {
          name: "pit",
          align: "center",
          label: this.$t('pit'),
          field: (row) => row.pay,
        },
        {
          name: "socialInsurance",
          align: "center",
          label: this.$t('socialInsurance'),
          field: (row) => row.pay,
        },
        {
          name: "healthInsurance",
          align: "center",
          label: this.$t('healthInsurance'),
          field: (row) => row.pay,
        },
        {
          name: "action",
          align: "center",
          label: this.$t('actions'),
          field: (row) => row.id,
        },
      ],
    };
  },
  methods: {
    ...mapActions("auth", ["useRefreshToken", "validateToken"]),

    async getEmployeeWithFilter(props) {
      try {
        this.loadingData = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        let { page, rowsPerPage } = props
          ? props.pagination
          : { page: 1, rowsPerPage: this.pagination.rowsPerPage };

        // Request API
        let result = await api
          .post(
            `/api/v1/person/pagination-salary?page=${page}&pageSize=${rowsPerPage}`,
            this.filter
          )
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
          this.mappingPagination(result);
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } finally {
        this.loadingData = false;
      }
    },
    async getDepartment() {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get("/api/v1/department")
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
        this.listDepartment = result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async getPosition() {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get("/api/v1/position")
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
        this.listPosition = result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    mappingPagination(resource) {
      this.listEmployee = resource.resource;

      this.listEmployee.forEach((row, index) => {
        row.index = index + 1;
      });

      this.pagination.page = resource.page;
      this.pagination.rowsPerPage = resource.pageSize;
      this.pagination.rowsNumber = resource.totalRecords;

      this.pagination.firstPage = resource.firstPage;
      this.pagination.lastPage = resource.lastPage;
      this.pagination.nextPage = resource.nextPage;
      this.pagination.previousPage = resource.previousPage;
      this.pagination.totalPages = resource.totalPages;
    },
    showName(text) {
      if (text.length > 20) return `${text.slice(0, 20)}...`;
      else return text;
    },
    filterDepartment(val, update, abort) {
      update(() => {
        if (!val) {
          this.tempListDepartment = this.listDepartment.slice(0, 5);
        } else {
          let needle = val.toLowerCase();
          this.tempListDepartment = this.listDepartment.filter(
            (v) => v.name.toLowerCase().indexOf(needle) > -1
          );
        }
      });
    },
    filterPosition(val, update, abort) {
      update(() => {
        if (!val) {
          this.tempListPosition = this.listPosition.slice(0, 5);
        } else {
          let needle = val.toLowerCase();
          this.tempListPosition = this.listPosition.filter(
            (v) => v.name.toLowerCase().indexOf(needle) > -1
          );
        }
      });
    },
    async getEmployeeById(id) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get(`/api/v1/person/${id}`)
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

      return result?.resource;
    },
    convertDateTimeToDate(dateTime, stringFormat = "YYYY-MM-DD") {
      return date.formatDate(dateTime, stringFormat);
    },
    async openInsertPay(id) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      let tempDate = new Date(this.filter.date);
      tempDate.setDate(1);
      tempDate.setMonth(tempDate.getMonth() - 1);

      // Request API
      let result = await api
        .get(
          `/api/v1/pay?personId=${id}&date=${this.convertDateTimeToDate(
            tempDate
          )}`
        )
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

      if (result.resource)
        this.insertPayResource.baseSalary = result.resource.baseSalary;

      this.insertPayResource.date = this.filter.date;
      this.insertPayObj = null;
      this.insertPayObj = this.listEmployee.find((x) => x.id == id);
      this.insertPayResource.personId = id;
      this.insertPayResource.allowance = 0;
      this.insertPayResource.bonus = 0;

      this.insertPay = true;
    },
    async createPay() {
      try {
        if (
          !this.$refs.oneRef.validate() ||
          !this.$refs.twoRef.validate() ||
          !this.$refs.threeRef.validate()
        )
          return null;

        this.payProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .post(`/api/v1/pay`, this.insertPayResource)
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
          this.getEmployeeWithFilter();
          (this.insertPay = false),
            this.$q.notify({
              type: "positive",
              message: "Successfully added!",
            });
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } finally {
        this.payProcess = false;
      }
    },
    showOrHide(id) {
      let tempPerson = this.listEmployee.find((x) => x.id == id);

      if (tempPerson.pay) return true;
      else return false;
    },
    numberWithSpaces(x, isFloat = true) {
      if (isFloat) {
        let parts = x.toString().split(".");
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, " ");
        return parts.join(".");
      } else {
        return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ");
      }
    },
    openDeletePay(id) {
      this.idDelete = id;
      this.deletePay = true;
    },
    async deletePayAsync() {
      try {
        this.deleteProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .delete(`/api/v1/pay/${this.idDelete}`)
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
          await this.getEmployeeWithFilter();
          this.deletePay = false;

          this.$q.notify({
            type: "positive",
            message: "Successfully deleted!",
          });
        } else {
          this.$q.notify({
            type: "negative",
            message: result.message[0],
          });
        }
      } finally {
        this.deleteProcess = false;
      }
    }
  },
  computed: {
    ...mapGetters("auth", ["getRole", "getInformation"]),

    getNameDelete() {
      let tempEmployee = this.listEmployee.filter((x) => x.id == this.idDelete);

      let name = tempEmployee[0]?.firstName;

      return name ? this.showName(name) : "";
    },
  },
  async created() {
    this.filter.date = this.convertDateTimeToDate(Date.now());

    let isValid = await this.validateToken();
    if (!isValid) this.$router.replace("/login");

    await Promise.all([
      this.getEmployeeWithFilter(),
      this.getPosition(),
      this.getDepartment(),
    ]);
  },
  mounted() {
    const $q = useQuasar();
  },
});
</script>

<style lang="scss" scoped>
.container {
  position: relative;

  .table-component {
    position: relative;

    .table-content {
      /* height or max-height is important */
      height: 600px;
      width: 100%;
    }
  }
}
</style>

<style lang="scss">
.table-content {
  .q-table__top,
  .q-table__bottom,
  thead tr:first-child th {
    /* bg color is important for th; just specify one */
    background-color: $accent !important;
  }

  thead tr th {
    position: sticky;
    z-index: 99;
    font-size: 14px !important;
    font-family: Poppins-Medium !important;
  }

  tbody tr td {
    font-size: 14px !important;
  }

  thead tr:first-child th {
    top: 0;
  }

  /* this is when the loading indicator appears */
  &.q-table--loading thead tr:last-child th {
    /* height of all previous header rows */
    top: 0;
  }
}

/* Scroll bar */
::-webkit-scrollbar {
  width: 10px;
  height: 10px;
}

::-webkit-scrollbar-thumb {
  background: $accent;
}

::-webkit-scrollbar-corner {
  background: $grey;
}
</style>
