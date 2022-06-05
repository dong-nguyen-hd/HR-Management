<template>
  <q-page class="full-height full-width flex flex-center">
    <div class="container full-height full-width">
      <q-dialog v-model="showDelete" :persistent="deleteProcess">
        <q-card>
          <q-card-section class="row items-center">
            <span class="text-h6">Delete "{{ getNameDelete }}"?</span>
          </q-card-section>

          <q-separator />

          <q-card-section>
            <span
              >This can’t be undone and it will be removed from database.</span
            >
          </q-card-section>

          <q-card-actions align="right">
            <q-btn
              flat
              :disable="deleteProcess"
              label="Cancel"
              color="primary"
              v-close-popup
            />
            <q-btn
              flat
              label="Delete"
              color="negative"
              @click="deleteGroup"
              :loading="deleteProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <q-dialog v-model="show" :persistent="true">
        <q-card style="width: 400px">
          <q-card-section class="row items-center">
            <span v-show="!showEdit" class="text-h6">Add Project</span>
            <span v-show="showEdit" class="text-h6">Edit Project</span>
          </q-card-section>

          <q-separator />

          <q-card-section>
            <div class="q-mt-sm">
              <q-input
                ref="oneRef"
                standout
                clearable
                maxlength="250"
                v-model="groupResource.name"
                type="text"
                label="Name:"
                :label-color="colorFocusGroup[0]"
                @focus="colorFocusGroup[0] = 'white'"
                @blur="colorFocusGroup[0] = ''"
                :rules="[(val) => !!val || 'Name is required']"
                lazy-rules="ondemand"
                hide-bottom-space
              >
              </q-input>
            </div>

            <div class="q-mt-sm">
              <q-input
                ref="twoRef"
                standout
                clearable
                autogrow
                maxlength="250"
                v-model="groupResource.description"
                type="text"
                label="Description:"
                :label-color="colorFocusGroup[1]"
                @focus="colorFocusGroup[1] = 'white'"
                @blur="colorFocusGroup[1] = ''"
                :rules="[(val) => !!val || 'Description is required']"
                lazy-rules="ondemand"
                hide-bottom-space
              >
              </q-input>
            </div>

            <div class="q-mt-sm">
              <q-input
                ref="threeRef"
                standout
                clearable
                v-model="groupResource.teamSize"
                type="number"
                label="Team Size:"
                :label-color="colorFocusGroup[2]"
                @focus="colorFocusGroup[2] = 'white'"
                @blur="colorFocusGroup[2] = ''"
                :rules="[(val) => !!val || 'Team Size is required']"
                lazy-rules="ondemand"
                hide-bottom-space
              >
              </q-input>
            </div>

            <div class="q-mt-sm">
              <q-input
                ref="fourRef"
                standout
                maxlength="500"
                v-model="groupResource.startDate"
                type="text"
                placeholder="YYYY-MM-DD"
                mask="####-##-##"
                stack-label
                label="Start Date:"
                :label-color="colorFocusGroup[3]"
                @focus="colorFocusGroup[3] = 'white'"
                @blur="colorFocusGroup[3] = ''"
                :rules="[
                  (val) => !!val || 'Start Date is required',
                  (val) => validateDate(val) || 'Start Date is invalid',
                ]"
                lazy-rules="ondemand"
                hide-bottom-space
              >
                <template v-slot:append>
                  <q-icon name="event" class="cursor-pointer">
                    <q-popup-proxy
                      cover
                      transition-show="scale"
                      transition-hide="scale"
                    >
                      <q-date
                        v-model="groupResource.startDate"
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
                ref="fiveRef"
                standout
                maxlength="500"
                v-model="groupResource.endDate"
                type="text"
                placeholder="YYYY-MM-DD"
                mask="####-##-##"
                stack-label
                label="End Date:"
                :label-color="colorFocusGroup[4]"
                @focus="colorFocusGroup[4] = 'white'"
                @blur="colorFocusGroup[4] = ''"
                :rules="[(val) => validateDate(val) || 'End Date is invalid']"
                lazy-rules="ondemand"
                hide-bottom-space
              >
                <template v-slot:append>
                  <q-icon name="event" class="cursor-pointer">
                    <q-popup-proxy
                      cover
                      transition-show="scale"
                      transition-hide="scale"
                    >
                      <q-date v-model="groupResource.endDate" mask="YYYY-MM-DD">
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
              <q-select
                ref="sixRef"
                standout
                clearable
                multiple
                use-chips
                max-values="̀50"
                use-input
                v-model="groupResource.technologies"
                :options="listTechnology"
                :label="labelNameFocus[1]"
                option-value="id"
                option-label="name"
                emit-value
                map-options
                options-selected-class="text-accent"
                :label-color="colorFocusGroup[5]"
                :rules="[(val) => val?.length || 'Skill is required']"
                @filter="filterTechnology"
                @focus="
                  colorFocusGroup[5] = 'white';
                  labelNameFocus[1] = 'Search by skill';
                "
                @blur="
                  colorFocusGroup[5] = '';
                  labelNameFocus[1] = 'Skills:';
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
          </q-card-section>

          <q-card-actions align="right">
            <q-btn
              flat
              :disable="groupProcess"
              label="Cancel"
              color="primary"
              v-close-popup
            />
            <q-btn
              v-show="!showEdit"
              flat
              label="Add"
              color="info"
              @click="saveInsert"
              :loading="groupProcess"
            />
            <q-btn
              v-show="showEdit"
              flat
              label="Edit"
              color="info"
              @click="saveUpdate"
              :loading="groupProcess"
            />
          </q-card-actions>
        </q-card>
      </q-dialog>

      <div class="table-component full-height full-width flex flex-center q-px-md">
        <div class="new-item q-mb-md flex justify-end full-width">
          <q-btn @click="openInsert" color="primary" label="New Project" />
        </div>

        <q-table
          class="table-content"
          :rows="listGroup"
          :columns="headerTable"
          row-key="id"
          flat
          bordered
          dark
          :loading="loadingData"
          v-model:pagination="pagination"
          :rows-per-page-options="[5, 10, 15, 20]"
          @request="getGroupWithFilter"
        >
          <template v-slot:header-cell-name="props">
            <q-th :props="props">
              <div :style="`min-width: ${widthOfName};`">
                <q-input
                  @update:model-value="getGroupWithFilter(false)"
                  dark
                  dense
                  standout
                  debounce="400"
                  v-model="filter.name"
                  input-class="text-right"
                  :label="labelNameFocus[0]"
                  :label-color="labelColorFocus[0]"
                  @focus="
                    labelColorFocus[0] = 'black';
                    labelNameFocus[0] = 'Search by name';
                    widthOfName = '170px';
                  "
                  @blur="
                    labelColorFocus[0] = 'white';
                    labelNameFocus[0] = props.col.label;
                    widthOfName = '100px';
                  "
                >
                  <template v-slot:append>
                    <q-icon v-if="!filter.name" name="search" />
                    <q-icon
                      v-else
                      name="clear"
                      class="cursor-pointer"
                      @click="
                        filter.name = '';
                        getGroupWithFilter(false);
                      "
                    />
                  </template>
                </q-input>
              </div>
            </q-th>
          </template>

          <template v-slot:header-cell-available="props">
            <q-th :props="props">
              <div>
                <q-checkbox
                  left-label
                  dense
                  dark
                  v-model="filter.available"
                  color="accent"
                  @update:model-value="getGroupWithFilter(false)"
                  label="Available"
                />
              </div>
            </q-th>
          </template>

          <template v-slot:body-cell-action="props">
            <q-td :props="props">
              <div>
                <q-btn
                  style="width: 60px"
                  dense
                  color="white"
                  text-color="black"
                  label="Edit"
                  @click="openEdit(props.value)"
                />
              </div>
              <div class="q-mt-sm">
                <q-btn
                  style="width: 60px"
                  dense
                  color="negative"
                  label="delete"
                  @click="
                    idDelete = props.value;
                    showDelete = true;
                  "
                />
              </div>
            </q-td>
          </template>

          <template v-slot:body-cell-available="props">
            <q-td :props="props">
              <q-icon
                v-show="!props.value"
                name="fas fa-check"
                color="positive"
                size="16px"
              />
            </q-td>
          </template>

          <template v-slot:body-cell-name="props">
            <q-td class="name-cell" :props="props">
              <div>{{ reduceText(props.value) }}</div>
            </q-td>
          </template>

          <template v-slot:body-cell-description="props">
            <q-td class="description-cell" :props="props">
              <div>{{ reduceText(props.value) }}</div>
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
import { mapActions } from "vuex";
import { useQuasar, date } from "quasar";
import { api } from "src/boot/axios";

export default defineComponent({
  name: "List Project",

  data() {
    return {
      labelColorFocus: ["white"],
      labelNameFocus: ["Name", "Skills:"],
      widthOfName: "100px",

      colorFocusGroup: [],

      loadingData: false,

      showDelete: false,
      idDelete: null,
      deleteProcess: false,

      show: false,
      showEdit: false,
      editObj: null,
      groupProcess: false,

      groupResource: {
        name: "",
        description: "",
        teamSize: null,
        startDate: null,
        endDate: null,
        technologies: [],
      },

      filter: {
        name: null,
        available: false,
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

      listGroup: [],
      listTechnology: [],
      tempListTechnoly: [],
      headerTable: [
        {
          name: "index",
          label: "#",
          align: "center",
          field: "index",
        },
        {
          name: "name",
          align: "left",
          label: "Name",
          field: "name",
        },
        {
          name: "description",
          align: "left",
          label: "Description",
          field: "description",
        },
        {
          name: "teamSize",
          align: "right",
          label: "Team Size",
          field: "teamSize",
        },
        {
          name: "available",
          align: "center",
          label: "Available",
          field: "isFinished",
        },
        {
          name: "action",
          align: "center",
          label: "Actions",
          field: (row) => row.id,
        },
      ],
    };
  },
  methods: {
    ...mapActions("auth", ["useRefreshToken", "validateToken"]),

    openInsert() {
      this.groupResource.name = "";
      this.groupResource.description = "";
      this.groupResource.teamSize = null;
      this.groupResource.startDate = null;
      this.groupResource.endDate = "";
      this.groupResource.technologies = [];

      this.showEdit = false;
      this.show = true;
    },
    async saveInsert() {
      if (
        !this.$refs.oneRef.validate() ||
        !this.$refs.twoRef.validate() ||
        !this.$refs.threeRef.validate() ||
        !this.$refs.fourRef.validate() ||
        !this.$refs.sixRef.validate()
      ) {
        return null;
      }

      if (this.groupResource.endDate) {
        if (!this.$refs.fiveRef.validate()) return null;
      } else {
        this.groupResource.endDate = null;
      }

      this.groupProcess = true;

      // Request API
      let result = await api
        .post(`/api/v1/group`, this.groupResource)
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
        if(this.listGroup.length >= 10) this.listGroup.pop();
        this.listGroup.unshift(result.resource);
        this.listGroup.forEach((row, index) => {
          row.index = index + 1;
        });

        this.show = false;

        this.$q.notify({
          type: "positive",
          message: "Successfully added",
        });
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }

      this.groupProcess = false;
    },
    openEdit(id) {
      // Map obj
      this.editObj = this.listGroup.find((x) => x.id == id);
      this.groupResource.name = this.editObj.name;
      this.groupResource.description = this.editObj.description;
      this.groupResource.teamSize = this.editObj.teamSize;
      this.groupResource.startDate = this.editObj.startDate;
      this.groupResource.endDate = this.editObj.endDate;
      this.groupResource.technologies = this.editObj.technologies?.map(
        (x) => x.id
      );
      // Map list technology
      this.listTechnology = this.editObj.technologies;
      // Pop up
      this.showEdit = true;
      this.show = true;
    },
    async saveUpdate() {
      if (
        !this.$refs.oneRef.validate() ||
        !this.$refs.twoRef.validate() ||
        !this.$refs.threeRef.validate() ||
        !this.$refs.fourRef.validate() ||
        !this.$refs.sixRef.validate()
      ) {
        return null;
      }

      if (this.groupResource.endDate) {
        if (!this.$refs.fiveRef.validate()) return null;
      } else {
        this.groupResource.endDate = null;
      }

      this.groupProcess = true;

      // Request API
      let result = await api
        .put(`/api/v1/group/${this.editObj.id}`, this.groupResource)
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
        let index = this.listGroup.findIndex((x) => x.id == this.editObj.id);
        let rowNumber = this.listGroup[index].index;
        this.listGroup[index] = result.resource;
        this.listGroup[index].index = rowNumber;

        this.show = false;

        this.$q.notify({
          type: "positive",
          message: "Successfully updated",
        });
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
      
      this.groupProcess = false;
    },
    async filterTechnology(val, update, abort) {
      update(async () => {
        if (val.length < 2) this.listTechnology = this.tempListTechnoly;
        if (val.length >= 2) await this.findTechnology(val, false);
      });
    },
    async findTechnology(keyword, seedValue) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get(
          `/api/v1/technology/search?filterName=${
            !keyword ? "" : keyword.trim()
          }`
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
        if (seedValue) this.tempListTechnoly = result.resource;
        this.listTechnology = result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async getGroupWithFilter(props) {
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
            `/api/v1/group/pagination?page=${page}&pageSize=${rowsPerPage}`,
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
    mappingPagination(resource) {
      this.listGroup = resource.resource;

      this.listGroup.forEach((row, index) => {
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
    reduceText(text) {
      if (!text) return "";

      if (text.length > 200) return `${text.slice(0, 200)}...`;
      else return text;
    },
    async deleteGroup() {
      try {
        this.deleteProcess = true;

        let isValid = await this.validateToken();
        if (!isValid) this.$router.replace("/login");

        // Request API
        let result = await api
          .delete(`/api/v1/group/${this.idDelete}`)
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
          await this.getGroupWithFilter(false);

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
    validateDate(dateTarget) {
      return date.isValid(dateTarget);
    },
  },
  computed: {
    getNameDelete() {
      let tempEmployee = this.listGroup.filter((x) => x.id == this.idDelete);

      let name = tempEmployee[0]?.name;

      return name ? this.reduceText(name) : "";
    },
  },
  async created() {
    let isValid = await this.validateToken();
    if (!isValid) this.$router.replace("/login");

    await Promise.all([
      this.getGroupWithFilter(0),
      this.findTechnology("", true),
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

.name-cell{
  max-width: 200px;
  white-space: initial;
}

.description-cell{
  max-width: 400px;
  white-space: initial;
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
