/// <reference path="../core.ts" />

interface JQuery {
    multiselect(option: any): void;
}

class ProductMultiSelectionComponent {
    $: JQueryStatic;
    $dropdownProduct: JQuery;
    $hiddenProductIdsInput: JQuery;
    $multiSelectButtonProduct: JQuery;

    $dropdownProductDetail: JQuery;
    $hiddenProductDetailIdsInput: JQuery;
    $multiSelectButtonProductDetail: JQuery;

    isReady = false;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render() {
        this.renderProductDetailDropdown();
        this.renderProductDropdown();
        this.isReady = true;
    }

    /****** Product Dropdown ******/
    private renderProductDropdown() {
        const dropdownProductId = 'dropdown-product';
        this.$dropdownProduct = this.$('#' + dropdownProductId);
        const hasData = this.$dropdownProduct.data('hasdata');

        const productsInputId = 'product-ids';
        this.$hiddenProductIdsInput = this.$('#' + productsInputId);

        this.$dropdownProduct.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            nonSelectedText: hasData
                ? this.$dropdownProduct.data('label-selectproducts')
                : this.$dropdownProduct.data('label-nodata'),
            nSelectedText: ` ${this.$dropdownProduct.data('label-selected')}`,
            allSelectedText: this.$dropdownProduct.data('label-allproducts'),
            includeSelectAllOption: hasData,
            onChange: (option: any, checked: boolean) => this.onChangeProduct(option, checked),
            onSelectAll: (option: any, checked: boolean) => this.onChangeProduct(option, checked),
            language: this.$dropdownProduct.data('language'),
            enableCollapsibleOptGroups: true,
            optionClass: (element) => this.optionClass(element)
        });

        this.$dropdownProduct.next().find('.multiselect-container li').removeClass('hidden');
        this.$multiSelectButtonProduct = this.$dropdownProduct.parent().find('.multiselect.btn');

        this.$dropdownProductDetail.length && this.shouldEnableProductDetailDropdown();
    }

    private optionClass(element: HTMLElement) {
        if (this.$(element).data('isnested') === 1) {
            return 'multiselect-nested-item';
        }
    }

    private onChangeProduct(option: any, checked: boolean) {
        if (this.isReady) {
            this.validateProductIds();
            this.$hiddenProductIdsInput.val(this.getProductIds());

            if (this.$dropdownProductDetail.length) {
                typeof option === 'boolean'
                    ? this.disableProductDetailDropdown(option)
                    : this.disableProductDetailItem(checked, option.context.value);
            }
        }
    }

    validateProductIds = () => {
        const isValid = this.$dropdownProduct.find('option:selected').length > 0;
        isValid
            ? this.$multiSelectButtonProduct.removeClass('error')
            : this.$multiSelectButtonProduct.addClass('error');

        return isValid;
    };

    getProductIds = () => this.getSelectedItems(this.$dropdownProduct);

    /****** End of Product Dropdown ******/

    /****** Product Details Dropdown ******/
    private renderProductDetailDropdown() {
        const productDetailDropdownId = 'dropdown-product-detail';
        this.$dropdownProductDetail = this.$('#' + productDetailDropdownId);

        if (this.$dropdownProductDetail.length) {
            const resourcesInputId = 'resource-ids';
            this.$hiddenProductDetailIdsInput = this.$('#' + resourcesInputId);
            const selectAllText = this.$dropdownProductDetail.data('label-selectall');

            this.$dropdownProductDetail.multiselect({
                maxHeight: 300,
                numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
                nonSelectedText: this.$dropdownProductDetail.data('label-selectnone'),
                nSelectedText: ` ${this.$dropdownProductDetail.data('label-selected')}`,
                allSelectedText: selectAllText,
                selectAllText: selectAllText,
                includeSelectAllOption: this.$dropdownProductDetail.data('hasdata'),
                onChange: () => this.onChangeProductDetail(),
                onSelectAll: () => this.onChangeProductDetail(),
                language: this.$dropdownProductDetail.data('language')
            });

            this.$multiSelectButtonProductDetail = this.$dropdownProductDetail.parent().find('.multiselect.btn');
        }
    }

    private onChangeProductDetail() {
        this.$hiddenProductDetailIdsInput.val(this.getProductDetailIds());
    }

    private disableProductDetailDropdown(checked: boolean) {
        this.$dropdownProductDetail.multiselect(checked ? 'enable' : 'disable');

        this.$dropdownProductDetail.find('option').prop('selected', checked).prop('disabled', !checked);
        this.$dropdownProductDetail.multiselect('refresh');
    }

    private disableProductDetailItem(checked: boolean, productId: number) {
        const $options = this.$dropdownProductDetail.find(`option[data-productid="${productId}"]`);

        if ($options.length) {
            $options.prop('selected', checked).prop('disabled', !checked);
            this.$dropdownProductDetail.multiselect('refresh');

            if (checked && this.$dropdownProductDetail.find('option:enabled').length) {
                this.$dropdownProductDetail.multiselect('enable');
            } else if (!this.$dropdownProductDetail.find('option:enabled').length) {
                this.$dropdownProductDetail.multiselect('disable');
            }

            this.onChangeProductDetail();
        }
    }

    private shouldEnableProductDetailDropdown() {
        let isEnableProductDetailDropdown = false;
        this.$dropdownProduct.find('option:selected').each((_, item) => {
            const filterOptions = this.$dropdownProductDetail.find(
                `option:disabled[data-productid="${this.$(item).val()}"]`
            );
            if (filterOptions.length) {
                filterOptions.prop('disabled', false);
                isEnableProductDetailDropdown = true;
            }
        });

        isEnableProductDetailDropdown && this.$dropdownProductDetail.multiselect('refresh');
    }

    getProductDetailIds = () => this.getSelectedItems(this.$dropdownProductDetail);
    /****** End of Product Details Dropdown ******/

    private getSelectedItems($dropdown: JQuery) {
        const selected = [];
        $dropdown.find('option:selected').each((_, item) => selected.push(this.$(item).val()));

        return selected.join(',');
    }
}
