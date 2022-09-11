<template>
  <q-page class="full-height full-width flex flex-center">
    <div class="container full-height full-width">
      <q-dialog v-model="showDelete" :persistent="deleteProcess">
        <q-card>
          <q-card-section class="row items-center">
            <span class="text-h6">{{ $t('delete') }} {{ getNameDelete }}?</span>
          </q-card-section>

          <q-separator />

          <q-card-section>
            <span
              >{{ $t('confirmDelete') }}</span
            >
          </q-card-section>

          <q-card-actions align="right">
            <q-btn
              flat
              :disable="deleteProcess"
              :label="$t('cancel')"
              color="primary"
              v-close-popup
            />
            <q-btn
              flat
              :label="$t('btnDelete')"
              color="negative"
              @click="deleteEmployee"
              :loading="deleteProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <q-dialog v-model="showEdit" persistent full-width full-height>
        <q-layout view="hHh lpR fFf" container class="bg-white">
          <q-header class="bg-accent">
            <q-toolbar>
              <q-toolbar-title></q-toolbar-title>
              <q-btn
                class="q-mr-lg"
                flat
                round
                dense
                icon="visibility"
                @click="openViewDialog(editObj.id)"
              />
              <q-btn flat round dense icon="close" @click="closeEditDialog" />
            </q-toolbar>
          </q-header>

          <q-page-container>
            <q-page>
              <edit-employee
                v-if="showEdit"
                @updateSuccess="updateReceive"
                :statusUpdateTransfer="statusUpdate"
                :employeeTransfer="editObj"
              />
            </q-page>
          </q-page-container>

          <q-footer class="bg-accent text-white">
            <q-toolbar class="flex flex-center">
              <q-btn
                :loading="statusUpdate"
                dense
                color="primary"
                :label="$t('btnSave')"
                class="q-px-lg"
                @click="saveUpdate"
              />
            </q-toolbar>
          </q-footer>
        </q-layout>
      </q-dialog>

      <q-dialog v-model="showInsert" persistent full-width full-height>
        <q-layout view="hHh lpR fFf" container class="bg-white">
          <q-header class="bg-accent">
            <q-toolbar>
              <q-toolbar-title></q-toolbar-title>
              <q-btn flat round dense icon="close" @click="closeInsertDialog" />
            </q-toolbar>
          </q-header>

          <q-page-container>
            <q-page>
              <new-employee
                v-if="showInsert"
                @insertSuccess="insertReceive"
                :statusInsertTransfer="statusInsert"
              />
            </q-page>
          </q-page-container>

          <q-footer class="bg-accent text-white">
            <q-toolbar class="flex flex-center">
              <q-btn
                :loading="statusInsert"
                dense
                color="primary"
                :label="$t('btnSave')"
                class="q-px-lg"
                @click="saveInsert"
              />
            </q-toolbar>
          </q-footer>
        </q-layout>
      </q-dialog>

      <q-dialog v-model="showTimesheet" full-width full-height>
        <q-layout view="hHh lpR fFf" container class="bg-white">
          <q-header class="bg-accent">
            <q-toolbar>
              <q-toolbar-title></q-toolbar-title>
              <q-btn
                flat
                round
                dense
                icon="close"
                @click="showTimesheet = false"
              />
            </q-toolbar>
          </q-header>

          <q-page-container>
            <q-page>
              <div class="fit absolute">
                <q-splitter class="fit" v-model="splitterModel">
                  <template v-slot:before>
                    <div class="q-pa-md row fit justify-center">
                      <q-date
                        class="fit"
                        landscape
                        v-model="dateTimesheet"
                        :events="addEventsTimesheet"
                        :event-color="addColorTimesheet"
                        @navigation="changeDateTimesheet"
                      />
                    </div>
                  </template>

                  <template v-slot:after>
                    <div class="q-pa-md">
                      <div class="text-h4 q-mb-md">
                        {{ convertDateTimeToDate(timesheet.date, "YYYY/MM") }}
                      </div>
                      <div>
                        {{ $t('totalWorkDay') }} {{ timesheet.workDay }} /
                        {{ timesheet.totalWorkDay }}
                      </div>
                      <div class="column">
                        <q-chip color="light-green-6" text-color="white">
                          {{ $t('fullDay') }}
                        </q-chip>
                        <q-chip color="amber-6" text-color="white">
                          {{ $t('halfDay') }}
                        </q-chip>
                        <q-chip color="red-6" text-color="white">
                          {{ $t('absent') }} {{ timesheet.absent }}
                        </q-chip>
                        <q-chip color="indigo-6" text-color="white">
                          {{ $t('holiday') }} {{ timesheet.holiday }}
                        </q-chip>
                        <q-chip color="cyan-6" text-color="white">
                          {{ $t('dayOff') }}
                        </q-chip>
                        <q-chip color="grey-6" text-color="white">
                          {{ $t('unpaidLeave') }} {{ timesheet.unpaidLeave }}
                        </q-chip>
                        <q-chip color="yellow-6" text-color="white">
                          {{ $t('paidLeave') }} {{ timesheet.paidLeave }}
                        </q-chip>
                      </div>
                    </div>
                  </template>
                </q-splitter>
              </div>
            </q-page>
          </q-page-container>
        </q-layout>
      </q-dialog>

      <div
        class="table-component full-height full-width flex flex-center q-px-md"
      >
        <div class="new-item q-mb-md flex justify-end full-width">
          <q-file
            outlined
            :loading="loadingUploadTimesheet"
            dense
            class="q-ml-md"
            style="overflow: hidden; max-height: 40px"
            input-style="width: 170px;"
            max-total-size="10485760"
            v-model="timesheetFile"
            accept=".xlsx"
            :display-value="
              timesheetFile ? $t('timesheetUploaded') : $t('timesheetHere')
            "
            @update:model-value="importTimesheet"
          >
            <template v-slot:prepend>
              <q-icon name="attach_file" />
            </template>
          </q-file>

          <q-btn
            class="q-ml-md"
            @click="openInsert"
            color="primary"
            :label="$t('newEmployee')"
            unelevated
          />
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
                        {{ $t('noResults') }}
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
              <q-fab
                color="white"
                text-color="black"
                icon="add"
                direction="left"
                style='z-index: 1'
              >
                <q-fab-action
                  color="negative"
                  text-color="white"
                  icon="delete"
                  @click="
                    idDelete = props.value;
                    showDelete = true;
                  "
                />
                <q-fab-action
                  color="positive"
                  text-color="white"
                  icon="edit"
                  @click="openEdit(props.value)"
                />
                <q-fab-action
                  color="info"
                  text-color="white"
                  icon="event"
                  @click="openShowTimesheet(props.value)"
                />
              </q-fab>
            </q-td>
          </template>

          <template v-slot:body-cell-fullName="props">
            <q-td :props="props">
              {{ showName(props.value) }}
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
import EditEmployee from "src/pages/Employee/EditEmployee.vue";
import NewEmployee from "src/pages/Employee/NewEmployee.vue";

export default defineComponent({
  name: "List Employee",

  components: {
    "edit-employee": EditEmployee,
    "new-employee": NewEmployee,
  },

  data() {
    return {
      labelColorFocus: ["white", "white", "white", "white"],
      labelNameFocus: [this.$t('staffID'), this.$t('fullName'), this.$t('department'), this.$t('position')],

      widthOfStaffId: "80px",
      widthOfFullName: "130px",
      widthOfDepartment: "100px",
      widthOfPosition: "100px",

      timesheetFile: null,
      splitterModel: 50,
      dateTimesheet: "",
      timesheet: {
        absent: 0,
        date: "",
        holiday: 0,
        id: 0,
        paidLeave: 0,
        timesheetJSON: "",
        totalWorkDay: 0,
        unpaidLeave: 0,
        workDay: 0,
      },
      showTimesheet: false,
      personTimesheetId: 0,
      loadingUploadTimesheet: false,

      loadingData: false,

      showDelete: false,
      idDelete: null,
      deleteProcess: false,

      showEdit: false,
      editObj: null,
      statusUpdate: false,

      showInsert: false,
      statusInsert: false,

      filter: {
        staffId: null,
        positionId: null,
        departmentId: null,
        firstName: null,
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

    openInsert() {
      this.showInsert = true;
    },
    closeInsertDialog() {
      this.showInsert = false;
      this.statusInsert = false;
    },
    saveInsert() {
      this.statusInsert = true;
    },
    async insertReceive(value) {
      if (value) {
        this.statusInsert = false;

        let result = await this.getEmployeeById(value);
        if (this.listEmployee.length >= 10) this.listEmployee.pop();
        this.listEmployee.unshift(result);
        this.listEmployee.forEach((row, index) => {
          row.index = index + 1;
        });

        // Update pagination
        this.pagination.rowsNumber = this.pagination.rowsNumber + 1;
      } else {
        this.statusInsert = false;
      }
    },
    async getEmployee() {
      try {
        this.loadingData = true;

        // Request API
        let result = await api
          .get(`/api/v1/person?page=1&pageSize=10`)
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
            `/api/v1/person/pagination?page=${page}&pageSize=${rowsPerPage}`,
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
    async deleteEmployee() {
      try {
        this.deleteProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .delete(`/api/v1/person/${this.idDelete}`)
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
          await this.getEmployeeWithFilter(false);

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
        this.showDelete = false;
      }
    },
    openEdit(id) {
      this.editObj = this.listEmployee.find((x) => x.id == id);
      this.showEdit = true;
    },
    saveUpdate() {
      this.statusUpdate = true;
    },
    async updateReceive(value) {
      if (value) {
        let result = await this.getEmployeeById(this.editObj.id);

        this.statusUpdate = false;
        this.showEdit = false;

        let index = this.listEmployee.findIndex((x) => x.id == this.editObj.id);
        let numberIndex = this.listEmployee[index].index;
        this.listEmployee[index] = Object.assign({}, result);
        this.listEmployee[index].index = numberIndex;
        this.editObj = null;
      } else {
        this.statusUpdate = false;
      }
    },
    async closeEditDialog() {
      let result = await this.getEmployeeById(this.editObj.id);

      let index = this.listEmployee.findIndex((x) => x.id == this.editObj.id);
      let numberIndex = this.listEmployee[index].index;
      this.listEmployee[index] = Object.assign({}, result);
      this.listEmployee[index].index = numberIndex;
      this.editObj = null;

      this.statusUpdate = false;
      this.showEdit = false;
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
    async importTimesheet() {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      this.loadingUploadTimesheet = true;

      let fd = new FormData();
      fd.append("file", this.timesheetFile);

      // Request API import timesheet
      let result = await api
        .post(`/api/v1/timesheet/import`, fd)
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
        this.$q.notify({
          type: "positive",
          message: "Successfully uploaded",
        });
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.loadingUploadTimesheet = false;
    },
    async openShowTimesheet(id, dateInput = "") {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      this.personTimesheetId = id;

      if (!dateInput) {
        dateInput = this.convertDateTimeToDate(Date.now(), "YYYY/MM/DD");
        this.dateTimesheet = dateInput;
      }

      this.timesheet.date = dateInput;
      this.timesheet.timesheetJSON = "";

      this.showTimesheet = true;

      // Request API import timesheet
      let result = await api
        .get(`/api/v1/timesheet?personId=${id}&date=${dateInput}`)
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

      if (result.success && result.resource) {
        this.timesheet = Object.assign({}, result.resource);
        this.timesheet.timesheetJSON = JSON.parse(this.timesheet.timesheetJSON);
      }
    },
    async changeDateTimesheet(view) {
      let dateString = `${view.year}/${view.month}/1`;
      let date = this.convertDateTimeToDate(dateString);
      await this.openShowTimesheet(this.personTimesheetId, date);
    },
    addEventsTimesheet(date) {
      if (!this.timesheet.timesheetJSON) return false;
      let parts = this.convertDateTimeToDate(date, "YYYY/M/D").split("/");

      let indexOfDay = parseInt(parts[2]);
      let dayValue = this.timesheet.timesheetJSON[indexOfDay - 1];
      if (dayValue != "-") {
        return true;
      } else return false;
    },
    addColorTimesheet(date) {
      let parts = this.convertDateTimeToDate(date, "YYYY/M/D").split("/");

      let indexOfDay = parseInt(parts[2]);
      let dayValue = this.timesheet.timesheetJSON[indexOfDay - 1];

      if (dayValue == "W") return "light-green-6";
      if (dayValue == "R") return "amber-6";
      if (dayValue == "A") return "red-6";
      if (dayValue == "H") return "indigo-6";
      if (dayValue == "S") return "grey-6";
      if (dayValue == "V") return "yellow-6";
      if (dayValue == "O") return "cyan-6";

      return "white";
    },
    convertDateTimeToDate(dateTime, stringFormat = "YYYY-MM-DD") {
      return date.formatDate(dateTime, stringFormat);
    },
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
    let isValid = await this.validateToken();
    if (!isValid) this.$router.replace("/login");

    await Promise.all([
      this.getEmployee(),
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

.height-view {
  min-height: 52px;
  border: 1px solid $primary;
  border-bottom: 0;
}

.height-view-sub {
  min-height: 44px;
  border: 1px solid $primary;
  border-bottom: 0;
}

.border-right {
  border-right: 1px solid $primary;
}

.education-view {
  min-height: 71px;
  border: 1px solid $primary;
  border-bottom: 0;
}

.border-image-view {
  border-top: 1px solid $primary;
  border-right: 1px solid $primary;
}

.project-view {
  border: 1px solid $primary;
  border-bottom: 0;
}

.project-bottom-view {
  border-bottom: 1px solid $primary;
}

.project-height-view {
  min-height: 30px;
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
