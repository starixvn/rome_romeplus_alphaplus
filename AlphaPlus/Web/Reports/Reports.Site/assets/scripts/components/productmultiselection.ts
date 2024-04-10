/// <reference path="../core.ts" />

interface JQuery {
    multiselect(option: any): void;
    multiselect(method: string, value: any): void;
}

class ProductMultiSelectionComponent {
    $dropdownProduct: JQuery;
    $hiddenProductIdsInput: JQuery;
    $multiSelectButtonProduct: JQuery;

    $dropdownProductDetail: JQuery;
    $hiddenProductDetailIdsInput: JQuery;
    $multiSelectButtonProductDetail: JQuery;

    isReady = false;
    numberDisplayed: 1;

    onChangeProductCallback: Function;

    render(onChangeProduct: Function = null) {
        this.onChangeProductCallback = onChangeProduct;
        this.renderProductDetailDropdown();
        this.renderProductDropdown();
        this.isReady = true;
    }

    /****** Product Dropdown ******/
    private renderProductDropdown() {
        const dropdownProductId = 'dropdown-product';
        this.$dropdownProduct = $('#' + dropdownProductId);
        const hasData = this.$dropdownProduct.data('hasdata');

        const productsInputId = 'product-ids';
        this.$hiddenProductIdsInput = $('#' + productsInputId);

        this.$dropdownProduct.multiselect({
            maxHeight: 300,
            numberDisplayed: this.numberDisplayed,
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

        if (this.$dropdownProductDetail.length) {
            this.deselectInvalidProductIds();
            this.shouldEnableProductDetailDropdown();
        }
    }

    private optionClass(element: HTMLElement) {
        if ($(element).data('isnested') === 1) {
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

            this.onChangeProductCallback && this.onChangeProductCallback();
        }
    }

    private deselectInvalidProductIds() {
        const selectedProductIds = this.getSelectedItems(this.$dropdownProduct);
        const productIds = this.$hiddenProductIdsInput.val().split(',');
        if (selectedProductIds.length !== productIds.length) {
            const unselectedProductIds = selectedProductIds.filter((id) => !productIds.includes(id));
            if (unselectedProductIds.length) {
                this.$dropdownProduct.multiselect('deselect', unselectedProductIds);
            }
        }
    }

    validateProductIds = (): boolean => {
        const isValid = this.$dropdownProduct.find('option:selected').length > 0;
        isValid
            ? this.$multiSelectButtonProduct.removeClass('error')
            : this.$multiSelectButtonProduct.addClass('error');

        return isValid;
    };

    getProductIds = (): string => this.getSelectedItems(this.$dropdownProduct).join(',');

    /****** End of Product Dropdown ******/

    /****** Product Details Dropdown ******/
    private renderProductDetailDropdown() {
        const productDetailDropdownId = 'dropdown-product-detail';
        this.$dropdownProductDetail = $('#' + productDetailDropdownId);

        if (this.$dropdownProductDetail.length) {
            const resourcesInputId = 'resource-ids';
            this.$hiddenProductDetailIdsInput = $('#' + resourcesInputId);
            const selectAllText = this.$dropdownProductDetail.data('label-selectall');

            this.$dropdownProductDetail.multiselect({
                maxHeight: 300,
                numberDisplayed: this.numberDisplayed,
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
        this.isReady && this.$hiddenProductDetailIdsInput.val(this.getProductDetailIds());
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
                `option:disabled[data-productid="${$(item).val()}"]`
            );
            if (filterOptions.length) {
                filterOptions.prop('disabled', false);
                isEnableProductDetailDropdown = true;
            }
        });

        isEnableProductDetailDropdown && this.$dropdownProductDetail.multiselect('refresh');
    }

    getProductDetailIds = (): string => this.getSelectedItems(this.$dropdownProductDetail).join(',');
    /****** End of Product Details Dropdown ******/

    private getSelectedItems($dropdown: JQuery) {
        const selected = [];
        $dropdown.find('option:selected').each((_, item) => selected.push($(item).val()));

        return selected;
    }
}
