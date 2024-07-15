class TransferListPaging {
    init(submitHandler: Function) {
        const totalPages = $('.pagingContainer').data('totalpages');
        const handlePageIndexChanged = function (pageIndex: number) {
            if (0 < pageIndex && pageIndex <= totalPages) {
                $('#PageIndex').val(pageIndex);
                submitHandler();
            }
        };

        $('.pagingSize').change(function () {
            $('#PageIndex').val(1);
            $('#PageSize').val(this.value);
            submitHandler();
        });

        $('.pagingContainer .icon:not(.disabled)').click(function () {
            handlePageIndexChanged($(this).data('pageindex'));
        });

        $('.pagingContainer .pagingCurrent')
            .on('input', function () {
                this.value = this.value.replace(/\D/g, '').replace(/^0+(?!$)/, '');
            })
            .keyup(function (event) {
                const key = event.which || event.keyCode;
                key === 13 && handlePageIndexChanged(+this.value);
            });
    }
}
