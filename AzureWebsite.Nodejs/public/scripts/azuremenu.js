$.fn.extend({
    treed: function (o) {

        var openedClass = 'fa-minus';
        var closedClass = 'fa-plus';

        if (typeof o != 'undefined') {
            if (typeof o.openedClass != 'undefined') {
                openedClass = o.openedClass;
            }
            if (typeof o.closedClass != 'undefined') {
                closedClass = o.closedClass;
            }
        };

        //initialize each of the top levels
        var tree = $(this);
        tree.addClass("tree");
        tree.find('li').has("ul").each(function () {
            var branch = $(this); //li with children ul
            branch.prepend("<i class='indicator fa-solid  " + closedClass + "'></i>");
            branch.addClass('branch');
            branch.on('click', function (e) {
                if (this == e.target) {
                    var icon = $(this).children('i:first');
                    icon.toggleClass(openedClass + " " + closedClass);
                    $(this).children().children().toggle();
                }
            })
            branch.children().children().toggle();
        });
        //fire event from the dynamically added icon
        tree.find('.branch .indicator').each(function () {
            $(this).on('click', function () {
                $(this).closest('li').click();
            });
        });
        //fire event to open branch if the li contains an anchor instead of text
        tree.find('.branch>a').each(function () {
            $(this).on('click', function (e) {
                $(this).closest('li').click();
                e.preventDefault();
            });
        });
        //fire event to open branch if the li contains a button instead of text
        tree.find('.branch>button').each(function () {
            $(this).on('click', function (e) {
                $(this).closest('li').click();
                e.preventDefault();
            });
        });
    }
});

function GetMenuFromAzureFunction()
{
    fetch(MainMenuApiUrl, { method: 'GET' })
    .then(response => response.json())
        .then(data => {
            var mainMenu = document.getElementById("mainMenu");
            data.menuItems.forEach(function (menuItem) {
                AppendMenuItem(mainMenu, menuItem);
            });
           
            $("#mainMenu").shieldTreeView();

    });
}

function AppendMenuItem(mainMenu, menuItem)
{
    var li = document.createElement('li');
    li.innerHTML = "<li class='nav-item'><a class='nav-link' href='#'><span>" + menuItem.title + "</span></a></li>";

    if (menuItem.menuItems && menuItem.menuItems.length > 0)
    {
        var subMenu = document.createElement('ul');
        menuItem.menuItems.forEach(function (subMenuItem) {
            AppendMenuItem(subMenu, subMenuItem);
        });
        li.append(subMenu);
    }
    mainMenu.append(li);
}

GetMenuFromAzureFunction();





