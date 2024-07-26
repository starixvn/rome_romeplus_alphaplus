class MemberCommissionView {
    init() {
        this.initComponents();
    }

    private initComponents() {
        const $ddlProduct = $('#ddl-product');
        $ddlProduct.multiselect({
            multiple: false,
            maxHeight: 300,
            onChange: function (abc) {
                console.dir($(abc).val());
            }
        });
    }
}

$(function () {
    new MemberCommissionView().init();
});
