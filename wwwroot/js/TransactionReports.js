let transactionList = [];
let categories = [];

class Transaction {
    constructor(id, amount, categoryId, category, accountId, account, date, isReoccuring, notes, accountUsersId, accountUsers) {
        this.id = id;
        this.amount = amount;
        this.categoryId = categoryId;
        this.category = category;
        this.accountId = accountId;
        this.account = account;
        this.date = date;
        this.isReoccuring = isReoccuring;
        this.notes = notes;
        this.accountUsersId = accountUsersId;
        this.accountUsers = accountUsers;
    }

    get TotalExpense() {

    }
}

function CreateCategories(list) {
    categories = list;
}
function CreateTransactions(list) {
    
    for (let i = 0; i < list.length; i++) {
        let tempTrans = new Transaction(list[i].Id, list[i].Amount, list[i].CategoryId, list[i].Category, list[i].AccountId, list[i].Account, list[i].Date, list[i].isReoccuring, list[i].Notes, list[i].AccountUsersId, list[i].AccountUsers);
        transactionList.push(tempTrans);
    }
}

function CalcTotalExpenseAndProfit(list) {
    let expense = 0.0;
    let profit = 0.0;
    for (let i = 0; i < list.length; i++) {
        if (list[i].amount < 0) {
            expense += list[i].amount;
        }
        else {
            profit += list[i].amount;
        }
    }
    document.getElementById('expenseTotal').innerHTML = "Total Expense: " + expense;
    document.getElementById('profitTotal').innerHTML = "Total Profit: " + profit;
    document.getElementById('netTotal').innerHTML = "Net Total: " + (profit + expense);
}

function GatherTransactionsByDate(startDate, endDate) {
    let tempTransactionList = [];
    for (let i = 0; i < transactionList.length; i++) {
        if (Date.parse(transactionList[i].date) <= endDate && Date.parse(transactionList[i].date) >= startDate) {
            tempTransactionList.push(transactionList[i])
        }
    }
    CalcTotalExpenseAndProfit(tempTransactionList);
    GatherCategoryChartData(tempTransactionList);
}

function GatherCategoryChartData(transList) {
    let categoryTransData = new Object;
    //initialize 'dictionay'
    for (let i = 0; i < categories.length; i++) {
        var tempName = categories[i].Name;
        categoryTransData[tempName] = 0;
    }

    for (let i = 0; i < transList.length; i++) {
        var name = transList[i].category.Name;
        var curVal = categoryTransData[name]
        categoryTransData[name] = (curVal + transList[i].amount);
    }

    OrganizeChartData(categoryTransData);
}

function GatherCategoryDataByMonth(startDate, endDate) {
    let masterObj = [];
    for (let x = 0; x < categories.length; x++) {
        let categoryId = categories[x].Id;
        let tempTransactionList = [];
        let tempDataObject = {}
        const categoryDataByMonth = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        for (let i = 0; i < transactionList.length; i++) {

            
            if (transactionList[i].categoryId == categoryId && Date.parse(transactionList[i].date) <= endDate && Date.parse(transactionList[i].date) >= startDate) {
                tempTransactionList.push(transactionList[i]);
            }
        }
        // want obkect like {data: data, label: lable}
        for (let i = 0; i < tempTransactionList.length; i++) {
            let tempDate = Date.parse(tempTransactionList[i].date);
            let tempMonth = new Date(tempDate).getMonth(); //0-11
            let tempValue = categoryDataByMonth[tempMonth];
            tempValue += tempTransactionList[i].amount;
            categoryDataByMonth[tempMonth] = tempValue;
        }
        tempDataObject.data = categoryDataByMonth;
        tempDataObject.label = categories[x].Name;
        masterObj.push(tempDataObject);
    }
    
    return masterObj;
    //return categoryDataByMonth;
}