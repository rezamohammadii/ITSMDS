import { API_ADDRESS } from './customVariable.js';



document.getElementById("userForm").addEventListener("submit", submitForm);
async function submitForm(event) {


    event.preventDefault();
    const ip = document.querySelector('[name="ipAddress"]').value.trim();
    const email = document.querySelector('[name="Email"]').value.trim();
    const password = document.querySelector('[name="Password"]').value.trim();
    const personalCode = document.querySelector('[name="PersonalCode"]').value.trim();
    const phone = document.querySelector('[name="PhoneNumber"]').value.trim();
    console.log(ip)
    let errors = [];

    const ipRegex = /^(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d{2}|[1-9]?\d)){3}$/;
    if (!ipRegex.test(ip)) {
        errors.push("🛑 IP وارد شده معتبر نیست.");
    }

    if (!email.includes("@") || !email.includes(".")) {
        errors.push("🛑 ایمیل معتبر وارد کنید.");
    }

    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$/;
    if (!passwordRegex.test(password)) {
        errors.push("🛑 رمز عبور باید حداقل ۸ کاراکتر و شامل حرف بزرگ، کوچک، عدد و علامت خاص باشد.");
    }


    if (!/^\d{4,6}$/.test(personalCode)) {
        errors.push("🛑 کد پرسنلی باید عددی و بین ۴ تا ۶ رقم باشد.");
    }


    if (!/^\d{8,11}$/.test(phone)) {
        errors.push("🛑 شماره تماس باید عددی و بین ۸ تا ۱۱ رقم باشد.");
    }


    const resultDiv = document.getElementById("result");
    if (errors.length > 0) {
        resultDiv.classList.remove("d-none", "alert-success");
        resultDiv.classList.add("alert-danger");
        resultDiv.innerHTML = errors.join("<br>");
    } else {
        resultDiv.classList.remove("d-none", "alert-danger");
        resultDiv.classList.add("alert-success");
        resultDiv.innerHTML = "✅ اطلاعات معتبر است. در حال ارسال...";

        const form = document.getElementById("userForm");
        const formData = new FormData(form);
        console.log(Object.fromEntries(formData.entries()));
        console.log(API_ADDRESS);

        const data = {};
        formData.forEach((value, key) => { data[key] = value });
        var url = API_ADDRESS + "api/user/create"
        try {
            const response = await fetch(url, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data)
            });

            if (response.ok) {
                document.getElementById("result").classList.remove("d-none");
                document.getElementById("result").innerText = "✅ اطلاعات ارسال شد";
                form.reset();
            } else {
                alert("❌ خطا در ارسال اطلاعات");
            }
        } catch (error) {
            console.error(error);
            alert("❌ خطا در ارتباط با سرور");
        }
    }

    
}
