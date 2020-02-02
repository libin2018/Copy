Vue.component('vue-pager', {
    template: '<div style="width:100%;text-align:center;margin-top: 10px;"><ul class="pagination" v-show="totalpages>0" style="margin-bottom: 0px;margin-top: 0px;">'
        + '<li class="disabled"><a>共{{totalpages}}页 of {{totalitems}}条纪录</a></li>'
                            + '<li v-bind:class="{disabled:pageindex==1}"><a href="javascript:;" v-on:click="change(1)">首页</a></li>'
                            + '<li v-bind:class="{disabled:pageindex<=1}"><a href="#" v-on:click="change(pageindex-1)">上页</a></li>'
                            + '<li v-for="btn in btns" v-bind:class="{active:btn==pageindex}"><a href="javascript:;" v-on:click="change(btn)">{{btn}}</a></li>'
                            + '<li v-bind:class="{disabled:pageindex>=totalpages}"><a href="javascript:;" v-on:click="change(pageindex+1)">下页</a></li>'
                            + '<li v-bind:class="{disabled:pageindex==totalpages}"><a href="javascript:;" v-on:click="change(totalpages)">尾页</a></li>'
                        + '</ul>'
                         + '<ul class="pagination" v-if="totalpages==0" style="margin-bottom: 0px;margin-top: 0px;">'
                            + '<li class="disabled"><a>暂无数据</a></li>'
                         + '</ul></div>',
    props: {
        totalpages: 0,
        totalitems: 0,
        pageindex: Number,
    },
    methods: {
        change: function (index) {
            if (index > this.totalpages || index < 1) {
                return;
            }
            this.$emit('change', index);
        },
        range: function (beg, end) {
            var ret = [];
            while (beg < end) {
                ret.push(beg);
                beg++;
            }
            return ret;
        }
    },
    computed: {
        btns: function () {
            if (this.totalpages <= 5) {
                return this.range(1, this.totalpages + 1);
            } else {
                if (this.pageindex < 4) {
                    return this.range(1, 6);
                } else if (this.pageindex > 3 && this.pageindex < this.totalpages - 1) {
                    return this.range(this.pageindex - 2, this.pageindex + 3);
                } else {
                    return this.range(this.totalpages - 4, this.totalpages + 1);
                }
            }
        }
    },
})
