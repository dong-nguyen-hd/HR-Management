<template>
  <q-page class="fit">
    <div class="flex absolute-center container">
      <div class="row top-side">
        <div class="col-6 left-side flex flex-center">
          <div class="fit q-py-lg">
            <q-scroll-area style="height: 500px">
              <div class="avatar flex flex-center column">
                <q-avatar size="100px">
                  <img :src="imageURL" />
                </q-avatar>
                <q-file
                  class="q-mt-sm cursor-pointer"
                  dense
                  tabindex="1"
                  clearable
                  standout
                  input-style="width: 150px;"
                  max-total-size="5242880"
                  :display-value="
                    imageFile ? 'Image is uploaded' : 'Upload image here!'
                  "
                  v-model="imageFile"
                  accept=".jpg, .png, .gif, .bmp, image/*"
                  @update:model-value="previewImage"
                  @clear="clearTempImage"
                  @rejected="onRejected"
                >
                  <template v-slot:prepend>
                    <q-icon name="attach_file" />
                  </template>
                </q-file>
              </div>

              <div class="first-name q-mt-lg q-px-lg">
                <q-input
                  ref="firstNameRef"
                  tabindex="2"
                  standout
                  clearable
                  maxlength="500"
                  v-model="employeeInfor.firstName"
                  type="text"
                  label="First Name:"
                  :label-color="labelColorFocus[0]"
                  @focus="labelColorFocus[0] = 'white'"
                  @blur="labelColorFocus[0] = ''"
                  :rules="[(val) => !!val || 'First Name is required']"
                  hide-bottom-space
                >
                </q-input>
              </div>

              <div class="last-name q-mt-sm q-px-lg">
                <q-input
                  ref="lastNameRef"
                  tabindex="3"
                  standout
                  clearable
                  maxlength="500"
                  v-model="employeeInfor.lastName"
                  type="text"
                  label="Last Name:"
                  :label-color="labelColorFocus[1]"
                  @focus="labelColorFocus[1] = 'white'"
                  @blur="labelColorFocus[1] = ''"
                  :rules="[(val) => !!val || 'Last Name is required']"
                  hide-bottom-space
                ></q-input>
              </div>

              <div class="yob q-mt-sm q-px-lg">
                <q-input
                  ref="yobRef"
                  tabindex="4"
                  standout
                  maxlength="500"
                  v-model="employeeInfor.yearOfBirth"
                  type="text"
                  placeholder="MM/DD/YYYY"
                  mask="##/##/####"
                  stack-label
                  label="Year Of Birth:"
                  :label-color="labelColorFocus[2]"
                  @focus="labelColorFocus[2] = 'white'"
                  @blur="labelColorFocus[2] = ''"
                  :rules="[(val) => !!val || 'YOB is required']"
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
                          v-model="employeeInfor.yearOfBirth"
                          mask="MM/DD/YYYY"
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

              <div class="gender q-mt-sm q-px-lg">
                <q-select
                  standout
                  ref="genderRef"
                  tabindex="5"
                  v-model="employeeInfor.gender"
                  :options="listGender"
                  label="Gender:"
                  option-value="id"
                  option-label="name"
                  emit-value
                  map-options
                  options-selected-class="text-accent"
                  :rules="[(val) => !!val || 'Gender is required']"
                  hide-bottom-space
                  :label-color="labelColorFocus[3]"
                  @focus="labelColorFocus[3] = 'white'"
                  @blur="labelColorFocus[3] = ''"
                >
                </q-select>
              </div>

              <div class="description q-mt-sm q-px-lg">
                <q-input
                  ref="descriptionRef"
                  standout
                  tabindex="6"
                  clearable
                  maxlength="500"
                  v-model="employeeInfor.description"
                  type="text"
                  autogrow
                  label="Description:"
                  :label-color="labelColorFocus[4]"
                  @focus="labelColorFocus[4] = 'white'"
                  @blur="labelColorFocus[4] = ''"
                  hide-bottom-space
                ></q-input>
              </div>

              <div class="office q-mt-sm q-px-lg">
                <q-select
                  standout
                  ref="officeRef"
                  tabindex="7"
                  v-model="employeeInfor.officeId"
                  :options="tempListOffice"
                  label="Office:"
                  option-value="id"
                  option-label="name"
                  emit-value
                  map-options
                  options-selected-class="text-accent"
                  @filter="filterOffice"
                  fill-input
                  hide-selected
                  use-input
                  :rules="[(val) => !!val || 'Office is required']"
                  hide-bottom-space
                  :label-color="labelColorFocus[5]"
                  @focus="labelColorFocus[5] = 'white'"
                  @blur="labelColorFocus[5] = ''"
                  ><template v-slot:no-option>
                    <q-item>
                      <q-item-section class="text-grey">
                        No results
                      </q-item-section>
                    </q-item>
                  </template>
                </q-select>
              </div>

              <div class="email q-mt-sm q-px-lg">
                <q-input
                  ref="emailRef"
                  tabindex="8"
                  standout
                  clearable
                  maxlength="500"
                  v-model="employeeInfor.email"
                  type="email"
                  label="Email:"
                  :label-color="labelColorFocus[6]"
                  @focus="labelColorFocus[6] = 'white'"
                  @blur="labelColorFocus[6] = ''"
                  hide-bottom-space
                >
                </q-input>
              </div>

              <div class="phone q-mt-sm q-px-lg">
                <q-input
                  ref="phoneRef"
                  mask="#########################"
                  standout
                  clearable
                  tabindex="9"
                  maxlength="25"
                  v-model="employeeInfor.phone"
                  type="text"
                  label="Phone:"
                  :label-color="labelColorFocus[7]"
                  @focus="labelColorFocus[7] = 'white'"
                  @blur="labelColorFocus[7] = ''"
                  hide-bottom-space
                >
                </q-input>
              </div>
            </q-scroll-area>
          </div>
        </div>
        <div class="col-6 right-side">
          <div class="fit q-pa-lg">
            <q-card>
              <q-tabs
                v-model="tab"
                dense
                outside-arrows
                mobile-arrows
                class="text-grey"
                active-color="primary"
                indicator-color="primary"
                align="justify"
                narrow-indicator
              >
                <q-tab
                  v-for="item in tabModel"
                  :key="item.id"
                  :name="item.id"
                  :label="item.name"
                />
              </q-tabs>

              <q-separator />

              <q-tab-panels v-model="tab" animated>
                <q-tab-panel name="1" class="tab-skill flex flex-center">
                  <div class="full-width relative-position flex flex-center">
                    <q-btn-group rounded flat unelevated>
                      <q-btn
                        color="primary"
                        label="<"
                        @click="orderTabPrev()"
                        size="md"
                      />
                      <q-btn
                        :color="booleanTab(1) ? 'primary' : 'grey'"
                        label="Skill"
                        @click="toggleTab(1)"
                        size="md"
                      >
                        <q-tooltip anchor="top middle" self="center middle">
                          {{ booleanTab(1) ? "Hide" : "Show" }}
                        </q-tooltip></q-btn
                      >
                      <q-btn
                        color="primary"
                        label=">"
                        @click="orderTabNext()"
                        size="md"
                      />
                    </q-btn-group>

                    <q-btn
                      size="12px"
                      class="absolute-top-right"
                      icon="add"
                      color="primary"
                      round
                      unelevated
                      @click="openDialog(1)"
                    />
                  </div>

                  <div class="fit">
                    <q-scroll-area class="q-mt-md" style="height: 378px">
                      <q-card
                        v-for="(category, index) in listTempCategory"
                        :key="index"
                        bordered
                        class="skill-inside"
                      >
                        <q-card-section horizontal class="row">
                          <q-card-section class="col-10">
                            <div class="q-mb-sm">
                              <q-badge
                                class="text-subtitle2"
                                :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                              >
                                <span>{{ category?.name }}:</span>
                              </q-badge>
                            </div>

                            <span
                              v-for="skill in category?.technologies"
                              :key="skill?.id"
                              class="q-mr-sm"
                            >
                              <q-badge
                                :color="index % 2 == 0 ? 'blue-4' : 'teal-4'"
                                style="font-size: 14px"
                              >
                                {{ skill?.name }}
                              </q-badge>
                            </span>
                          </q-card-section>

                          <q-card-actions vertical class="col-2 justify-around">
                            <q-btn
                              @click="orderSkillPrev(category)"
                              flat
                              round
                              :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                              icon="keyboard_arrow_up"
                            />

                            <q-fab
                              class="q-my-sm"
                              icon="more_horiz"
                              direction="left"
                              padding="sm"
                              unelevated
                              :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                            >
                              <q-fab-action
                                @click="editSkill(category)"
                                color="info"
                                icon="edit"
                                ><q-tooltip
                                  anchor="top middle"
                                  self="center middle"
                                >
                                  Edit
                                </q-tooltip></q-fab-action
                              >
                              <q-fab-action 
                              @click="deleteSkill(category)"
                              color="negative"
                              icon="delete"
                                ><q-tooltip
                                  anchor="top middle"
                                  self="center middle"
                                >
                                  Delete
                                </q-tooltip></q-fab-action
                              >
                            </q-fab>

                            <q-btn
                              flat
                              @click="orderSkillNext(category)"
                              round
                              :color="index % 2 == 0 ? 'blue-10' : 'teal-8'"
                              icon="keyboard_arrow_down"
                            />
                          </q-card-actions>
                        </q-card-section>
                      </q-card>
                    </q-scroll-area>
                  </div>

                  <q-dialog v-model="dialogToggle[1]" persistent>
                    <q-card style="width: 400px">
                      <q-card-section class="row items-center">
                        <span class="text-h6">{{
                          showEditBtn ? "Edit Skill" : "Add Skill"
                        }}</span>
                      </q-card-section>

                      <q-separator />

                      <q-card-section>
                        <div class="q-mt-sm">
                          <q-select
                            standout
                            ref="categoryRef"
                            v-model="tempCategoryPersonResource.categoryId"
                            :options="tempListCategory"
                            :label="labelNameFocusSkill[0]"
                            :option-disable="
                              (item) =>
                                getDisableCategory.some((x) => x == item.id)
                                  ? true
                                  : false
                            "
                            option-value="id"
                            option-label="name"
                            emit-value
                            map-options
                            options-selected-class="text-accent"
                            @filter="filterCategory"
                            fill-input
                            input-debounce="200"
                            hide-selected
                            use-input
                            :rules="[(val) => !!val || 'Office is required']"
                            hide-bottom-space
                            :label-color="colorFocusSkill[0]"
                            @focus="
                              colorFocusSkill[0] = 'white';
                              labelNameFocusSkill[0] = 'Search by category';
                            "
                            @blur="
                              colorFocusSkill[0] = '';
                              labelNameFocusSkill[0] = 'Category:';
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

                        <div class="q-mt-sm">
                          <q-select
                            ref="skillRef"
                            standout
                            clearable
                            multiple
                            use-chips
                            max-values="̀50"
                            use-input
                            :disable="
                              tempCategoryPersonResource.categoryId
                                ? false
                                : true
                            "
                            v-model="tempCategoryPersonResource.technologies"
                            :options="tempListSkillCategory"
                            :label="labelNameFocusSkill[1]"
                            option-value="id"
                            option-label="name"
                            emit-value
                            map-options
                            options-selected-class="text-accent"
                            :label-color="colorFocusSkill[1]"
                            :rules="[
                              (val) => val?.length || 'Skill is required',
                            ]"
                            @filter="filterSkillBelongWithCategory"
                            @focus="
                              colorFocusSkill[1] = 'white';
                              labelNameFocusSkill[1] = 'Search by skill';
                            "
                            @blur="
                              colorFocusSkill[1] = '';
                              labelNameFocusSkill[1] = 'Skill:';
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
                          label="Cancel"
                          color="primary"
                          v-close-popup
                        />
                        <q-btn
                          flat
                          label="Add"
                          color="info"
                          @click="addSkill"
                          v-show="!showEditBtn"
                        />
                        <q-btn
                          flat
                          label="Save"
                          color="info"
                          @click="saveSkill"
                          v-show="showEditBtn"
                        />
                      </q-card-actions>
                    </q-card>
                  </q-dialog>

                  <q-dialog v-model="deleteToggle[1]" persistent>
                    <q-card>
                      <q-card-section class="row items-center">
                        <span class="text-h6"
                          >Delete {{ tempDeleteSkill.name }} category?</span
                        >
                      </q-card-section>

                      <q-separator />

                      <q-card-section>
                        <span
                          >This can’t be undone and it will be removed.</span
                        >
                      </q-card-section>

                      <q-card-actions align="right">
                        <q-btn
                          flat
                          v-close-popup
                          label="Cancel"
                          color="primary"
                        />
                        <q-btn
                          flat
                          label="Delete"
                          color="negative"
                          @click="saveDeleteSkill"
                        />
                      </q-card-actions>
                    </q-card>
                  </q-dialog>
                </q-tab-panel>

                <q-tab-panel name="2">
                  <div class="tab-skill flex flex-center">
                    <q-btn-group rounded flat unelevated>
                      <q-btn
                        color="primary"
                        label="<"
                        @click="orderTabPrev()"
                      />
                      <q-btn color="primary" label="Project" disable />
                      <q-btn
                        color="primary"
                        label=">"
                        @click="orderTabNext()"
                      />
                    </q-btn-group>
                    Lorem ipsum dolor sit amet consectetur adipisicing elit.
                  </div>
                </q-tab-panel>

                <q-tab-panel name="3">
                  <div class="tab-skill flex flex-center">
                    <q-btn-group rounded flat unelevated>
                      <q-btn
                        color="primary"
                        label="<"
                        @click="orderTabPrev()"
                      />
                      <q-btn color="primary" label="Work History" disable />
                      <q-btn
                        color="primary"
                        label=">"
                        @click="orderTabNext()"
                      />
                    </q-btn-group>
                    Lorem ipsum dolor sit amet consectetur adipisicing elit.
                  </div>
                </q-tab-panel>

                <q-tab-panel name="4">
                  <div class="tab-skill flex flex-center">
                    <q-btn-group rounded flat unelevated>
                      <q-btn
                        color="primary"
                        label="<"
                        @click="orderTabPrev()"
                      />
                      <q-btn color="primary" label="Education" disable />
                      <q-btn
                        color="primary"
                        label=">"
                        @click="orderTabNext()"
                      />
                    </q-btn-group>
                    Lorem ipsum dolor sit amet consectetur adipisicing elit.
                  </div>
                </q-tab-panel>

                <q-tab-panel name="5">
                  <div class="tab-skill flex flex-center">
                    <q-btn-group rounded flat unelevated>
                      <q-btn
                        color="primary"
                        label="<"
                        @click="orderTabPrev()"
                      />
                      <q-btn color="primary" label="Certificate" disable />
                      <q-btn
                        color="primary"
                        label=">"
                        @click="orderTabNext()"
                      />
                    </q-btn-group>
                    Lorem ipsum dolor sit amet consectetur adipisicing elit.
                  </div>
                </q-tab-panel>
              </q-tab-panels>
            </q-card>
          </div>
        </div>
      </div>
      <div class="bottom-side flex flex-center">
        <q-btn
          tabindex="10"
          :loading="loadingSave"
          :disable="loadingSave"
          color="primary"
          label="Save"
          style="width: 100px"
        />
      </div>
    </div>
  </q-page>
</template>

<script>
import { defineComponent } from "vue";
import { mapGetters, mapActions } from "vuex";
import { useQuasar } from "quasar";
import { api } from "src/boot/axios";

export default defineComponent({
  name: "New Employee",

  data() {
    return {
      tab: "1",
      tabModel: [
        { id: "1", name: "Skill" },
        { id: "2", name: "Project" },
        { id: "3", name: "Work History" },
        { id: "4", name: "Education" },
        { id: "5", name: "Certificate" },
      ],

      avatarDefault: "",
      imageURL: null,
      imageFile: null,

      employeeInfor: {
        firstName: "",
        lastName: "",
        email: "",
        description: "",
        phone: "",
        yearOfBirth: "",
        officeId: "",
        gender: "",
        orderIndex: [1, 2, 3, 4, 5],

        categoryPersonResource: [],
        certificateResource: [],
        educationResource: [],
        projectResource: [],
        workHistoryResource: [],
      },

      tempCategoryPersonResource: {
        personId: 0,
        categoryId: 0,
        technologies: [],
      },
      listTempCategory: [],

      tempDeleteSkill: null,

      loadingSave: false,

      labelColorFocus: [],
      colorFocusSkill: [],
      labelNameFocusSkill: ["Category:", "Skill:"],
      showEditBtn: false,

      listGender: [
        {
          name: "Male",
          id: 1,
        },
        {
          name: "Female",
          id: 2,
        },
        {
          name: "Sexless",
          id: 3,
        },
      ],
      dialogToggle: [],
      deleteToggle: [],
      tempOrderIndex: [],
      tempListSkillCategory: [],
      tempListCategory: [],
      listCategory: [],
      tempListOffice: [],
      listOffice: [],
    };
  },
  methods: {
    ...mapActions("auth", ["useRefreshToken", "validateToken"]),

    async findCategory(keyword, seedValue) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get(
          `/api/v1/category/search?filterName=${!keyword ? "" : keyword.trim()}`
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
        this.tempListCategory = result.resource;
        if (seedValue) this.listCategory = result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async getByCategoryId(id) {
      let isValid = await this.validateToken();
      if (!isValid) this.$router.replace("/login");

      // Request API
      let result = await api
        .get(`/api/v1/category/${id}`)
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
        let category = result.resource;
        this.tempListCategory = [];
        this.tempListCategory.unshift(category);
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async getOffice() {
      // Request API
      let result = await api
        .get("/api/v1/office")
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
        this.listOffice = result.resource;
      } else {
        this.$q.notify({
          type: "negative",
          message: result.message[0],
        });
      }
    },
    async getAvatarUrl() {
      // Request API
      let result = await api
        .get(`/api/v1/image/default-image`)
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
        this.avatarDefault = result?.resource?.originalImagePath;
      }
    },
    previewImage() {
      this.imageURL = this.imageFile
        ? URL.createObjectURL(this.imageFile)
        : null;
    },
    clearTempImage() {
      this.imageURL = this.avatarDefault;
    },
    onRejected(rejectedEntries) {
      this.$q.notify({
        type: "negative",
        message: `Image size must lower than 5 MB`,
      });
    },
    filterOffice(val, update, abort) {
      update(() => {
        if (!val) {
          this.tempListOffice = this.listOffice.slice(0, 5);
        } else {
          let needle = val.toLowerCase();
          this.tempListOffice = this.listOffice.filter(
            (v) => v.name.toLowerCase().indexOf(needle) > -1
          );
        }
      });
    },
    filterCategory(val, update, abort) {
      update(async () => {
        if (val.length > 0 && val.length < 2)
          this.tempListCategory = this.listCategory;
        if (val.length >= 2) await this.findCategory(val, false);
      });
    },
    filterSkillBelongWithCategory(val, update, abort) {
      update(() => {
        let temp = this.getSkillBelongWithCategory();
        if (!val) {
          this.tempListSkillCategory = temp;
        } else {
          let needle = val.toLowerCase();
          this.tempListSkillCategory = temp.filter(
            (v) => v.name.toLowerCase().indexOf(needle) > -1
          );
        }
      });
    },
    getSkillBelongWithCategory() {
      let categoryId = this.tempCategoryPersonResource.categoryId;

      let obj = this.tempListCategory.find((x) => x.id == categoryId);

      return obj.technologies;
    },
    openDialog(index) {
      if (index == 1) {
        this.tempCategoryPersonResource.categoryId = 0;
        this.tempCategoryPersonResource.technologies = [];
      }

      this.dialogToggle[index] = true;
    },
    orderTabNext() {
      let count = this.tabModel.length;
      let index = this.tabModel.findIndex((x) => x.id == this.tab);

      if (index == count - 1) {
        let temp = this.tabModel.slice(0, index);
        temp.unshift(this.tabModel[index]);
        this.tabModel = temp;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let temp = this.tabModel[i];
            this.tabModel[i] = this.tabModel[i + 1];
            this.tabModel[i + 1] = temp;
            break;
          }
        }
      }
    },
    orderTabPrev() {
      let count = this.tabModel.length;
      let index = this.tabModel.findIndex((x) => x.id == this.tab);

      if (index == 0) {
        let temp = this.tabModel.slice(1);
        temp.push(this.tabModel[index]);
        this.tabModel = temp;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let temp = this.tabModel[i];
            this.tabModel[i] = this.tabModel[i - 1];
            this.tabModel[i - 1] = temp;
            break;
          }
        }
      }
    },
    toggleTab(val) {
      let index = this.employeeInfor.orderIndex.findIndex(
        (x) => x == parseInt(val)
      );

      let oldIndex = this.tempOrderIndex[parseInt(val)];

      if (index >= 0) {
        this.employeeInfor.orderIndex.splice(index, 1);
        this.tempOrderIndex[parseInt(val)] = index;
      } else if (oldIndex >= 0) {
        let left = this.employeeInfor.orderIndex.slice(0, oldIndex);
        let right = this.employeeInfor.orderIndex.slice(oldIndex);

        this.employeeInfor.orderIndex = left.concat(parseInt(val), right);
      } else {
        this.employeeInfor.orderIndex.push(parseInt(val));
      }
    },
    booleanTab(val) {
      return this.employeeInfor.orderIndex.some((x) => x == parseInt(val));
    },
    addSkill() {
      if (
        !this.$refs.categoryRef.validate() ||
        !this.$refs.skillRef.validate()
      ) {
        return null;
      }

      // List for send to server
      Object.assign({}, this.getInformation);
      this.employeeInfor.categoryPersonResource.unshift(
        Object.assign({}, this.tempCategoryPersonResource)
      );

      // List for print
      let category = this.tempListCategory.find(
        (x) => x.id == this.tempCategoryPersonResource.categoryId
      );
      category.technologies = category.technologies.filter((x) =>
        this.tempCategoryPersonResource.technologies.includes(x.id)
      );
      this.listTempCategory.unshift(category);

      this.dialogToggle[1] = false;
    },
    async editSkill(item) {
      await this.getByCategoryId(item.id);

      let temp = [];
      item.technologies.forEach((x) => temp.push(x.id));

      this.tempCategoryPersonResource.categoryId = item.id;
      this.tempCategoryPersonResource.technologies = temp;

      this.tempListSkillCategory = this.getSkillBelongWithCategory();

      this.showEditBtn = true;
      this.dialogToggle[1] = true;
    },
    saveSkill() {
      // Set list API
      let index = this.employeeInfor.categoryPersonResource.findIndex(
        (x) => x.categoryId == this.tempCategoryPersonResource.categoryId
      );
      this.employeeInfor.categoryPersonResource[index] = Object.assign(
        {},
        this.tempCategoryPersonResource
      );

      // Mapping with print-list
      let category = this.tempListCategory.find(
        (x) => x.id == this.tempCategoryPersonResource.categoryId
      );
      category.technologies = category.technologies.filter((x) =>
        this.tempCategoryPersonResource.technologies.includes(x.id)
      );

      this.listTempCategory[index] = category;

      this.showEditBtn = false;
      this.dialogToggle[1] = false;
    },
    deleteSkill(item){
      this.tempDeleteSkill = item;
      this.deleteToggle[1] = true;
    },
    saveDeleteSkill(){
      // Set list API
      let index = this.employeeInfor.categoryPersonResource.findIndex(
        (x) => x.categoryId == this.tempDeleteSkill.id
      );
      this.employeeInfor.categoryPersonResource.splice(index, 1);

      // Set list temp print
      this.listTempCategory.splice(index, 1);

      this.deleteToggle[1] = false;
    },
    orderSkillNext(item){
      let count = this.employeeInfor.categoryPersonResource.length;
      // Set list API
      let index = this.employeeInfor.categoryPersonResource.findIndex(
        (x) => x.categoryId == item.id
      );

      if (index == count - 1) {
        let tempOne = this.employeeInfor.categoryPersonResource.slice(0, index);
        tempOne.unshift(this.employeeInfor.categoryPersonResource[index]);
        this.employeeInfor.categoryPersonResource = tempOne;

        let tempTwo = this.listTempCategory.slice(0, index);
        tempTwo.unshift(this.listTempCategory[index]);
        this.listTempCategory = tempTwo;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let tempOne = this.employeeInfor.categoryPersonResource[i];
            this.employeeInfor.categoryPersonResource[i] = this.employeeInfor.categoryPersonResource[i + 1];
            this.employeeInfor.categoryPersonResource[i + 1] = tempOne;

            let tempTwo = this.listTempCategory[i];
            this.listTempCategory[i] = this.listTempCategory[i + 1];
            this.listTempCategory[i + 1] = tempTwo;

            break;
          }
        }
      }
    },
    orderSkillPrev(item){
      let count = this.employeeInfor.categoryPersonResource.length;
      // Set list API
      let index = this.employeeInfor.categoryPersonResource.findIndex(
        (x) => x.categoryId == item.id
      );

      if (index == 0) {
        let tempOne = this.employeeInfor.categoryPersonResource.slice(1);
        tempOne.push(this.employeeInfor.categoryPersonResource[index]);
        this.employeeInfor.categoryPersonResource = tempOne;

        let tempTwo = this.listTempCategory.slice(1);
        tempTwo.push(this.listTempCategory[index]);
        this.listTempCategory = tempTwo;
      } else {
        for (let i = 0; i < count; i++) {
          if (i == index) {
            let tempOne = this.employeeInfor.categoryPersonResource[i];
            this.employeeInfor.categoryPersonResource[i] = this.employeeInfor.categoryPersonResource[i - 1];
            this.employeeInfor.categoryPersonResource[i - 1] = tempOne;

            let tempTwo = this.listTempCategory[i];
            this.listTempCategory[i] = this.listTempCategory[i - 1];
            this.listTempCategory[i - 1] = tempTwo;

            break;
          }
        }
      }
    }
  },
  computed: {
    ...mapGetters("auth", ["getInformation"]),
    getDisableCategory() {
      let temp = [];
      this.employeeInfor.categoryPersonResource.forEach((x) =>
        temp.push(x.categoryId)
      );

      return temp;
    },
    getCategoryId() {
      return this.tempCategoryPersonResource.categoryId;
    },
  },
  async created() {
    let isValid = await this.validateToken();
    if (!isValid) this.$router.replace("/login");

    await Promise.all([
      this.findCategory(false, true),
      this.getOffice(),
      this.getAvatarUrl(),
    ]);

    this.imageURL = this.avatarDefault;
  },
  watch: {
    getCategoryId: function (newVal, oldVal) {
      if (oldVal && newVal) this.tempCategoryPersonResource.technologies = [];
    },
  },
  mounted() {
    const $q = useQuasar();
  },
});
</script>

<style lang="scss" scoped>
.container {
  min-height: inherit - 50px;
  height: calc(100% - 50px);
  width: calc(100% - 50px);
  background-color: $accent;
  border-radius: 10px;
  .top-side {
    position: absolute;
    width: 100%;
    height: 86%;
    top: 0;
  }

  .bottom-side {
    position: absolute;
    width: 100%;
    height: 14%;
    bottom: 0;
  }
}

.skill-inside {
  margin: 16px;
  &:last-child {
    margin: 16px 16px 0 16px;
  }
  &:first-child {
    margin: 0 16px 16px 16px;
  }
}
</style>
