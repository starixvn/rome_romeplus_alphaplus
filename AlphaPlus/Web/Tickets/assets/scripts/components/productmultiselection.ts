interface JQuery {
    multiselect(option: any): void;
}

class ProductMultiSelectionComponent {
    $dropdownProduct: JQuery;
    $hiddenProductIdsInput: JQuery;
    $multiSelectButtonProduct: JQuery;
    onChangeProductCallback: Function;
    isReady = false;

    render(onChangeProduct: Function = null) {
        this.onChangeProductCallback = onChangeProduct;
        this.renderProductDropdown();
        this.isReady = true;
    }

    private renderProductDropdown() {
        const dropdownProductId = 'dropdown-product';
        this.$dropdownProduct = $('#' + dropdownProductId);
        const hasData = this.$dropdownProduct.data('hasdata');

        const productsInputId = 'product-ids';
        this.$hiddenProductIdsInput = $('#' + productsInputId);

        this.$dropdownProduct.multiselect({
            maxHeight: 300,
            numberDisplayed: 1,
            nonSelectedText: hasData
                ? this.$dropdownProduct.data('label-selectproducts')
                : this.$dropdownProduct.data('label-nodata'),
            nSelectedText: ` ${this.$dropdownProduct.data('label-selected')}`,
            allSelectedText: this.$dropdownProduct.data('label-allproducts'),
            includeSelectAllOption: hasData,
            onChange: () => this.onChangeProduct(),
            onSelectAll: () => this.onChangeProduct(),
            language: this.$dropdownProduct.data('language'),
            enableCollapsibleOptGroups: true,
            optionClass: (element) => this.optionClass(element)
        });

        this.$dropdownProduct.next().find('.multiselect-container li').removeClass('hidden');
        this.$multiSelectButtonProduct = this.$dropdownProduct.parent().find('.multiselect.btn');

        return this.$dropdownProduct;
    }

    private optionClass(element: HTMLElement) {
        if ($(element).data('isnested') === 1) {
            return 'multiselect-nested-item';
        }
    }

    private onChangeProduct() {
        if (this.isReady) {
            this.validateProductIds();
            this.$hiddenProductIdsInput.val(this.getProductIds());
            this.onChangeProductCallback && this.onChangeProductCallback();
        }
    }

    validateProductIds = (): boolean => {
        const isValid = this.$dropdownProduct.find('option:selected').length > 0;
        isValid
            ? this.$multiSelectButtonProduct.removeClass('error')
            : this.$multiSelectButtonProduct.addClass('error');

        return isValid;
    };

    getProductIds = () => {
        const selected = [];
        this.$dropdownProduct.find('option:selected').each(function () {
            selected.push(this.value);
        });

        return selected.join(',');
    };
}
