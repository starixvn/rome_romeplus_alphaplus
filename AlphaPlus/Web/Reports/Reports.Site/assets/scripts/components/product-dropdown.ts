class ProductDropdownComponent {
    private $dropdownProduct: JQuery;
    private $dropdownProductDetail: JQuery;

    render(products, productDetails, onDropdownHidden: Function) {
        this.renderProductDetailDropdown(productDetails, onDropdownHidden);
        this.renderProductDropdown(products, onDropdownHidden);
    }

    private renderProductDetailDropdown(productDetails, onDropdownHidden) {
        const $dropdown = $('#dropdown-product-detail');

        if (productDetails && productDetails.length) {
            this.$dropdownProductDetail = $dropdown.fanexMultiSelect({
                selectAllText: $dropdown.data('label-selectall'),
                selectedText: $dropdown.data('label-selected'),
                noneSelectedText: $dropdown.data('label-noneselected'),
                customSelectAllText: $dropdown.data('label-all') + ' (#)',
                showFilter: true,
                filterPlaceHolder: $dropdown.data('label-filterplaceholder'),
                noItemFoundText: $dropdown.data('label-noitemfound'),
                minWidth: 250,
                maxHeight: 250,
                showTooltip: false,
                listData: productDetails.map((item) => this.buildCheckboxItem(item, { productId: item.ProductId })),
                onLeave: () => {
                    const productIds = this.$dropdownProductDetail.fanexMultiSelect('getUncheckedProductIds');
                    if (productIds.length) {
                        this.$dropdownProduct.fanexMultiSelect('uncheck', productIds);
                    }

                    onDropdownHidden && onDropdownHidden();
                }
            });
        } else {
            $dropdown.parent().remove();
        }
    }

    private renderProductDropdown(products, onDropdownHidden) {
        const productIdWithSubProductIds = {};
        const uncheckedProductIds = [];
        const productList = products.map((item) => {
            const checkboxItem: any = this.buildCheckboxItem(item);

            if (item.Products) {
                const subProductIds = [];
                productIdWithSubProductIds[item.Id] = subProductIds;

                checkboxItem.items = item.Products.map((item) => {
                    subProductIds.push(item.Id);
                    item.Checked || uncheckedProductIds.push(item.Id);
                    return this.buildCheckboxItem(item);
                });
            } else {
                item.Checked || uncheckedProductIds.push(item.Id);
            }

            return checkboxItem;
        });

        const self = this;
        const $dropdown = $('#dropdown-product');
        this.$dropdownProduct = $dropdown.fanexMultiSelect({
            selectAllText: $dropdown.data('label-selectall'),
            selectedText: $dropdown.data('label-selected'),
            noneSelectedText: $dropdown.data('label-noneselected'),
            customSelectAllText: $dropdown.data('label-all') + ' (#)',
            showFilter: true,
            filterPlaceHolder: $dropdown.data('label-filterplaceholder'),
            noItemFoundText: $dropdown.data('label-noitemfound'),
            minWidth: 250,
            maxHeight: 250,
            showTooltip: false,
            listData: productList,
            onCheck: function () {
                const productId = this.value;
                const checked = this.checked;
                const subProductIds = productIdWithSubProductIds[productId];
                if (subProductIds) {
                    const productIdString = checked
                        ? self.$dropdownProduct.fanexMultiSelect('getCheckedValues')
                        : self.$dropdownProduct.fanexMultiSelect('getUncheckedValues');
                    productIdString.split(',').forEach(function (id) {
                        self.handleOnCheckProduct(id, checked);
                    });
                } else {
                    self.handleOnCheckProduct(productId, checked);
                }
            },
            onCheckAll: function () {
                const checked = this.checked;
                const checkedValues = self.$dropdownProduct.fanexMultiSelect('getCheckedValues');
                const uncheckedValues = self.$dropdownProduct.fanexMultiSelect('getUncheckedValues');

                if ((checked && !uncheckedValues.length) || (!checked && !checkedValues.length)) {
                    if (self.$dropdownProductDetail) {
                        self.$dropdownProductDetail.fanexMultiSelect('showAllItems', checked);
                        self.$dropdownProductDetail.fanexMultiSelect('disable', !checked);
                    }
                } else {
                    const productIdString = checked ? checkedValues : uncheckedValues;
                    productIdString.split(',').forEach(function (id) {
                        self.handleOnCheckProduct(id, checked);
                    });
                }
            },
            onLeave: function () {
                onDropdownHidden && onDropdownHidden();
            }
        });

        this.initState(uncheckedProductIds);
    }

    private initState(uncheckedProductIds) {
        if (uncheckedProductIds.length) {
            uncheckedProductIds.forEach((item) => this.handleOnCheckProduct(item, false));
        }
    }

    private handleOnCheckProduct(productId, checked) {
        const action = checked ? 'showItemsByCustomAttributes' : 'hideItemsByCustomAttributes';
        this.$dropdownProductDetail && this.$dropdownProductDetail.fanexMultiSelect(action, { productId });
    }

    private buildCheckboxItem(item, attributes = undefined) {
        return {
            value: item.Id,
            text: item.Name,
            checked: item.Checked,
            attributes
        };
    }

    getCheckedValues() {
        const productsAndResources = {
            productIds: this.$dropdownProduct.fanexMultiSelect('getCheckedValues'),
            resourceIds: this.$dropdownProductDetail
                ? this.$dropdownProductDetail.fanexMultiSelect('getCheckedValues')
                : undefined
        };

        this.$dropdownProduct.fanexMultiSelect('addError', productsAndResources.productIds.length === 0);
        return productsAndResources;
    }

    getUncheckedValues() {
        return {
            unselectedProductIds: this.$dropdownProduct.fanexMultiSelect('getUncheckedValues'),
            unselectedResourceIds: this.$dropdownProductDetail
                ? this.$dropdownProductDetail.fanexMultiSelect('getUncheckedValues')
                : undefined
        };
    }
}
