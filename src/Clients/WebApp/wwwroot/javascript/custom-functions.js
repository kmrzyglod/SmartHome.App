var rules_basic = {
    condition: 'AND',
    rules: [{
        id: 'price',
        operator: 'less',
        value: 10.25
    }, {
        condition: 'OR',
        rules: [{
            id: 'category',
            operator: 'equal',
            value: 2
        }, {
            id: 'category',
            operator: 'equal',
            value: 1
        }]
    }]
};

function renderJSON(jsonStr, divId) {
    setTimeout(() => {
            const formatter = new JSONFormatter(jsonStr);
            var container = document.getElementById(divId);
            if (container.children.length > 0) {
                container.replaceChild(formatter.render(), container.children);
                return;
            }
            container.appendChild(formatter.render());
        },
        1);
    return;
}

function initQueryBuilder(divId) {
    $('#' + divId).queryBuilder({
  
        filters: [{
            id: 'name',
            label: 'Name',
            type: 'string'
        }, {
            id: 'category',
            label: 'Category',
            type: 'integer',
            input: 'select',
            values: {
                1: 'Books',
                2: 'Movies',
                3: 'Music',
                4: 'Tools',
                5: 'Goodies',
                6: 'Clothes'
            },
            operators: ['equal', 'not_equal', 'in', 'not_in', 'is_null', 'is_not_null']
        }, {
            id: 'in_stock',
            label: 'In stock',
            type: 'integer',
            input: 'radio',
            values: {
                1: 'Yes',
                0: 'No'
            },
            operators: ['equal']
        }, {
            id: 'price',
            label: 'Price',
            type: 'double',
            validation: {
                min: 0,
                step: 0.01
            }
        }, {
            id: 'id',
            label: 'Identifier',
            type: 'string',
            placeholder: '____-____-____',
            operators: ['equal', 'not_equal'],
            validation: {
                format: /^.{4}-.{4}-.{4}$/
            }
        }],

        rules: rules_basic
    });
    return;
}